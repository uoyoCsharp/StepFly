using MiCake.Core;
using MiCake.Core.Util;
using Microsoft.Extensions.Logging;
using StepFly.Core;
using StepFly.Domain;
using StepFly.Dtos;
using StepFly.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Services.LeXin
{
    public class LeXinStepFlyService : ILeXinStepFlyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LeXinStepFlyService> _logger;
        private readonly IStepFlyUserRepository _userRepo;

        public LeXinStepFlyService(IHttpClientFactory httpClientFactory, IStepFlyUserRepository repository, ILogger<LeXinStepFlyService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _userRepo = repository;
            _logger = logger;
        }

        public async Task<OperateResult> LoginToSystem<T>(T loginIno, CancellationToken cancellationToken = default)
        {
            var loginModel = loginIno as LeXinLoginModel;
            if (loginModel == null)
                return OperateResult.Failed(null, string.Empty, "当前登录信息转换失败");

            var existUser = await _userRepo.FindByUserKeyInfoAsync(loginModel.UserKeyInfo);

            OperateResult operateResult = null;
            if (loginModel.Type == LeXinLoginType.AuthCode)
            {
                operateResult = await LoginWithAuthCode(loginModel.AuthCodeLoginInfo, existUser, cancellationToken);
            }
            else if (loginModel.Type == LeXinLoginType.Password)
            {
                operateResult = await LoginWithPassword(loginModel.PasswordLoginInfo, existUser, cancellationToken);
            }

            if (operateResult != null)
            {
                if (!operateResult.Succeeded)
                    return operateResult;

                var result = operateResult.Information.PlayLoad! as LeXinLoginAPISuccessModel
                    ?? throw new SoftlyMiCakeException($"转换{nameof(LeXinLoginAPISuccessModel)}返回结果出错");

                var userLoginInfo = JsonSerializer.Deserialize<LeXinUserLoginSuccessModel>(result.APIResponseData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                //保存当前用户的信息
                if (!result.AlreadyHasUser)
                {
                    var user = StepFlyUser.Create(loginModel.UserKeyInfo, loginModel.PasswordLoginInfo?.Password ?? "", userLoginInfo.UserId);
                    user.SetToken(result.Cookie, DateTimeHelper.ConvertStampToDateTime(userLoginInfo.ExpireAt, TimeStampType.ThirteenLength));
                    user.SetClientInfo(result.ClientInfo, null);

                    await _userRepo.AddAsync(user);
                    result.User = user;
                }
                else
                {
                    existUser.SetToken(result.Cookie, DateTimeHelper.ConvertStampToDateTime(userLoginInfo.ExpireAt, TimeStampType.ThirteenLength));
                    existUser.SetPassword(loginModel.PasswordLoginInfo?.Password);
                    //保证设备ID不为空
                    existUser.SetClientInfo(existUser.UserClientInfo, existUser.DeviceId);
                    result.User = existUser;
                }


                return OperateResult.Success(HttpStatusCode.OK.ToString(), "登录成功", result);
            }
            else
            {
                return OperateResult.Failed(null, "E0", "未找到对应的登录方式");
            }
        }

        // 通过验证码登录
        private async Task<OperateResult> LoginWithAuthCode(LeXinAuthCodeLoginModel loginInfo, StepFlyUser existUser, CancellationToken cancellationToken = default)
        {
            try
            {
                CheckValue.NotNullOrWhiteSpace(loginInfo.LoginName, nameof(loginInfo.LoginName));
                CheckValue.NotNullOrWhiteSpace(loginInfo.AuthCode, nameof(loginInfo.AuthCode));

                var httpClient = _httpClientFactory.CreateClient();

                var clientId = GetClientId(existUser);
                var url = GetAuthCodeLoginUrl(clientId);
                loginInfo.SetClientId(clientId);

                var jsonOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var content = new StringContent(JsonSerializer.Serialize(loginInfo, jsonOptions), Encoding.UTF8, "application/json");

                using var response = await httpClient.PostAsync(url, content, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation(await content.ReadAsStringAsync());
                _logger.LogInformation(responseContent);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "登录失败", responseContent);
                }

                if (!response.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    var apiResult = JsonSerializer.Deserialize<LeXinHttpResponse>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    if (apiResult.Code != 200)
                    {
                        return OperateResult.Failed(null, apiResult.Code.ToString(), apiResult.Msg);
                    }

                    _logger.LogInformation($"登录返回成功，但是并没有获取到Cookie & Code:{response.StatusCode} & ResponseContent:{responseContent}");
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "登录返回成功，但是并没有获取到Cookie", responseContent);
                }

                var currentCookie = string.Join("", cookies.Select(s => s.Split(";")[0]).ToArray());
                //得到当前登录成功的用户信息
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var element = jsonDoc.RootElement.GetProperty("data");

                LeXinLoginAPISuccessModel result = new LeXinLoginAPISuccessModel()
                {
                    AlreadyHasUser = existUser != null,
                    APIResponseData = element.GetRawText(),
                    ClientInfo = clientId,
                    Cookie = currentCookie
                };

                return OperateResult.Success(HttpStatusCode.OK.ToString(), "登录成功", result);
            }
            catch (Exception ex)
            {
                return OperateResult.Failed(ex, "尝试登录时发生错误", ex.Message);
            }
        }

        // 通过账号密码登录
        private async Task<OperateResult> LoginWithPassword(LeXinPasswordLoginModel loginInfo, StepFlyUser existUser, CancellationToken cancellationToken = default)
        {
            try
            {
                CheckValue.NotNullOrWhiteSpace(loginInfo.LoginName, nameof(loginInfo.LoginName));
                CheckValue.NotNullOrWhiteSpace(loginInfo.Password, nameof(loginInfo.Password));

                var httpClient = _httpClientFactory.CreateClient();

                var clientId = GetClientId(existUser);
                var url = GetPasswordLoginUrl(clientId);
                loginInfo.SetClientId(clientId);

                var jsonOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var content = new StringContent(JsonSerializer.Serialize(loginInfo, jsonOptions), Encoding.UTF8, "application/json");

                using var response = await httpClient.PostAsync(url, content, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation(await content.ReadAsStringAsync());
                _logger.LogInformation(responseContent);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "登录失败", responseContent);
                }

                if (!response.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    var apiResult = JsonSerializer.Deserialize<LeXinHttpResponse>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    if (apiResult.Code != 200)
                    {
                        return OperateResult.Failed(null, apiResult.Code.ToString(), apiResult.Msg);
                    }
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "登录返回成功，但是并没有获取到Cookie", responseContent);
                }

                var currentCookie = string.Join("", cookies.Select(s => s.Split(";")[0]).ToArray());
                //得到当前登录成功的用户信息
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var element = jsonDoc.RootElement.GetProperty("data");

                LeXinLoginAPISuccessModel result = new LeXinLoginAPISuccessModel()
                {
                    AlreadyHasUser = existUser != null,
                    APIResponseData = element.GetRawText(),
                    ClientInfo = clientId,
                    Cookie = currentCookie
                };

                return OperateResult.Success(HttpStatusCode.OK.ToString(), "登录成功", result);
            }
            catch (Exception ex)
            {
                return OperateResult.Failed(ex, "尝试登录时发生错误", ex.Message);
            }
        }

        public async Task<OperateResult> UpdateStepAsync(int stepNum, UpdateStepUser userInfo, CancellationToken cancellationToken = default)
        {
            try
            {
                CheckValue.NotNullOrWhiteSpace(userInfo.UserKeyInfo, "userInfo.UserKeyInfo");

                var user = await _userRepo.FindByUserKeyInfoAsync(userInfo.UserKeyInfo) ??
                    throw new SoftlyMiCakeException("在修改步数的时候没有找到对应的用户信息");

                var httpClient = _httpClientFactory.CreateClient();
                var url = GetUpdateStepUrl();

                var jsonOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var content = new StringContent(JsonSerializer.Serialize(GetUpdateStepModel(stepNum, user), jsonOptions), Encoding.UTF8, "application/json");
                content.Headers.Add("Cookie", user.TokenInfo);

                using var response = await httpClient.PostAsync(url, content, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation(await content.ReadAsStringAsync());
                _logger.LogInformation(responseContent);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "修改步数失败", responseContent);
                }

                var lexinResponse = JsonSerializer.Deserialize<LeXinHttpResponse>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (lexinResponse.Code != 200)
                {
                    return OperateResult.Failed(null, lexinResponse.Code.ToString(), "修改步数失败", lexinResponse.Msg);
                }

                return OperateResult.Success(HttpStatusCode.OK.ToString(), "修改步数成功", responseContent);
            }
            catch (Exception ex)
            {
                return OperateResult.Failed(ex, "尝试修改步数时产生错误", ex.Message);
            }
        }

        // 获取验证码登录的url
        private string GetAuthCodeLoginUrl(string clientId)
        {
            var requestId = Guid.NewGuid().ToString().Replace("-", "");
            var deviceModel = LeXinConfig.DefaultDeviceModel;
            var requestToken = Guid.NewGuid().ToString().Replace("-", "");
            var ts = DateTimeHelper.GetTimeStamp(DateTime.Now).ToString();

            return string.Format(LeXinConfig.LoginUseCodeUrl, requestId, deviceModel, requestToken, ts, clientId);
        }

        // 获取账号密码登录的url
        private string GetPasswordLoginUrl(string clientId)
        {
            var requestId = Guid.NewGuid().ToString().Replace("-", "");
            var deviceModel = LeXinConfig.DefaultDeviceModel;
            var requestToken = Guid.NewGuid().ToString().Replace("-", "");
            var ts = DateTimeHelper.GetTimeStamp(DateTime.Now).ToString();

            return string.Format(LeXinConfig.LoginWithPassword, requestId, deviceModel, requestToken, ts);
        }

        //获取修改步数的url
        private string GetUpdateStepUrl()
        {
            var requestId = Guid.NewGuid().ToString().Replace("-", "");
            var deviceModel = LeXinConfig.DefaultDeviceModel;
            var requestToken = Guid.NewGuid().ToString().Replace("-", "");
            var ts = DateTimeHelper.GetTimeStamp(DateTime.Now).ToString();

            return string.Format(LeXinConfig.UploadInfoUrl, requestId, deviceModel, requestToken, ts);
        }

        //获取ClientId
        private string GetClientId(StepFlyUser user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user!.UserClientInfo))
                return Guid.NewGuid().ToString().Replace("-", "");

            return user!.UserClientInfo;
        }

        /// <summary>
        /// 获取修改步数的模型，用于序列化提交
        /// </summary>
        private LeXinUpdateStepModel GetUpdateStepModel(int step, StepFlyUser user)
        {
            LeXinUpdateStepModel model = new LeXinUpdateStepModel();

            LeXinStepItem item = new LeXinStepItem();
            item.SetStep(step);
            item.SetUserId(user.UserSystemId);
            item.SetDeviceId(user.DeviceId);

            model.List = new List<LeXinStepItem>() { item };
            return model;
        }

        public async Task<OperateResult> GetBindingType(string userKey, CancellationToken cancellationToken = default)
        {
            var requestId = Guid.NewGuid().ToString().Replace("-", "");
            var url = string.Format(LeXinConfig.GetBindTypeUrl, requestId);
            try
            {
                var user = await _userRepo.FindByUserKeyInfoAsync(userKey) ??
                    throw new SoftlyMiCakeException("没有找到对应的用户信息");

                var httpClient = _httpClientFactory.CreateClient();
                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                content.Headers.Add("Cookie", user.TokenInfo);

                using var res = await httpClient.PostAsync(url, content, cancellationToken);
                res.EnsureSuccessStatusCode();
                var responseContent = await res.Content.ReadAsStringAsync();

                var apiResult = JsonSerializer.Deserialize<LeXinHttpResponse>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (apiResult.Code != 200)
                {
                    return OperateResult.Failed(null, apiResult.Code.ToString(), apiResult.Msg);
                }

                using var jsonDoc = JsonDocument.Parse(responseContent);
                var element = jsonDoc.RootElement.GetProperty("data");
                var playload = JsonSerializer.Deserialize<LeXinBindingType>(element.GetRawText(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return OperateResult.Success("200", "", playload);
            }
            catch (Exception ex)
            {
                return OperateResult.Failed(ex);
            }
        }
    }

    //乐心登录方法成功返回的结果
    class LeXinLoginAPISuccessModel
    {
        public string Cookie { get; set; }

        public bool AlreadyHasUser { get; set; }

        public string ClientInfo { get; set; }

        public string APIResponseData { get; set; }

        public StepFlyUser User { get; set; }
    }
}
