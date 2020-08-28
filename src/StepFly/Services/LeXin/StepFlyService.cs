using MiCake.Core;
using MiCake.Core.Util;
using Microsoft.Extensions.Logging;
using StepFly.Core;
using StepFly.Domain;
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
    public class StepFlyService : IStepFlyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<StepFlyService> _logger;
        private readonly IStepFlyUserRepository _userRepo;

        public StepFlyService(IHttpClientFactory httpClientFactory, IStepFlyUserRepository repository, ILogger<StepFlyService> logger)
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

            if (loginModel.Type == LeXinLoginType.AuthCode)
                return await LoginWithAuthCode(loginModel.AuthCodeLoginInfo, cancellationToken);

            return OperateResult.Failed(null, "E0", "未找到对应的登录方式");
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

        // 通过验证码登录
        private async Task<OperateResult> LoginWithAuthCode(LeXinAuthCodeLoginModel loginInfo, CancellationToken cancellationToken = default)
        {
            try
            {
                CheckValue.NotNullOrWhiteSpace(loginInfo.LoginName, nameof(loginInfo.LoginName));
                CheckValue.NotNullOrWhiteSpace(loginInfo.AuthCode, nameof(loginInfo.AuthCode));

                var existUser = await _userRepo.FindByUserKeyInfoAsync(loginInfo.LoginName);

                var clientId = existUser?.UserClientInfo; //查询数据库中是否已经有保存过该用户的信息
                bool hasClientId = !string.IsNullOrWhiteSpace(clientId);

                var httpClient = _httpClientFactory.CreateClient();
                var url = GetAuthCodeLoginUrl(hasClientId, ref clientId);

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
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "登录返回成功，但是并没有获取到Cookie", responseContent);
                }

                var currentCookie = string.Join("", cookies.Select(s => s.Split(";")[0]).ToArray());
                //得到当前登录成功的用户信息
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var element = jsonDoc.RootElement.GetProperty("data");
                var userLoginInfo = JsonSerializer.Deserialize<LeXinUserLoginSuccessModel>(element.GetRawText(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                //保存当前用户的信息
                if (existUser == null)
                {
                    var user = StepFlyUser.Create(loginInfo.LoginName, userLoginInfo.UserId);
                    user.SetToken(currentCookie, DateTimeHelper.ConvertStampToDateTime(userLoginInfo.ExpireAt, TimeStampType.ThirteenLength));
                    user.SetClientInfo(clientId);

                    await _userRepo.AddAsync(user);
                }
                else
                {
                    existUser.SetToken(currentCookie, DateTimeHelper.ConvertStampToDateTime(userLoginInfo.ExpireAt, TimeStampType.ThirteenLength));
                    await _userRepo.UpdateAsync(existUser);
                }

                return OperateResult.Success(HttpStatusCode.OK.ToString(), "登录成功", responseContent);
            }
            catch (Exception ex)
            {
                return OperateResult.Failed(ex, "尝试登录时发生错误", ex.Message);
            }
        }

        // 获取验证码登录的url
        private string GetAuthCodeLoginUrl(bool hasClientId, ref string clientId)
        {
            var requestId = Guid.NewGuid().ToString().Replace("-", "");
            var deviceModel = LeXinConfig.DefaultDeviceModel;
            var requestToken = Guid.NewGuid().ToString().Replace("-", "");
            var ts = DateTimeHelper.GetTimeStamp().ToString();
            var clientInfo = hasClientId ? clientId : Guid.NewGuid().ToString().Replace("-", "");

            return string.Format(LeXinConfig.LoginUseCodeUrl, requestId, deviceModel, requestToken, ts, clientInfo);
        }

        //获取修改步数的url
        private string GetUpdateStepUrl()
        {
            var requestId = Guid.NewGuid().ToString().Replace("-", "");
            var deviceModel = LeXinConfig.DefaultDeviceModel;
            var requestToken = Guid.NewGuid().ToString().Replace("-", "");
            var ts = DateTimeHelper.GetTimeStamp().ToString();

            return string.Format(LeXinConfig.UploadInfoUrl, requestId, deviceModel, requestToken, ts);
        }

        /// <summary>
        /// 获取修改步数的模型，用于序列化提交
        /// </summary>
        private LeXinUpdateStepModel GetUpdateStepModel(int step, StepFlyUser user)
        {
            var deviceId = "M_868994040231057"; //从数据库中获取到deviceID

            LeXinUpdateStepModel model = new LeXinUpdateStepModel();

            LeXinStepItem item = new LeXinStepItem();
            item.SetStep(step);
            item.SetUserId(user.UserSystemId);
            item.SetDeviceId(deviceId);

            model.List = new List<LeXinStepItem>() { item };
            return model;
        }
    }
}
