using MiCake.Core;
using MiCake.Core.Util;
using Microsoft.Extensions.Logging;
using StepFly.Core;
using StepFly.Domain;
using StepFly.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace StepFly.Services.XiaoMi
{
    public class XiaoMiStepFlyService : IXiaoMiStepFlyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<XiaoMiStepFlyService> _logger;
        private readonly IStepFlyUserRepository _userRepo;

        public XiaoMiStepFlyService(IHttpClientFactory httpClientFactory, IStepFlyUserRepository repository, ILogger<XiaoMiStepFlyService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _userRepo = repository;
            _logger = logger;
        }

        public Task<OperateResult> GetBindingType(string userKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<OperateResult> LoginToSystem<T>(T loginIno, CancellationToken cancellationToken = default)
        {
            var loginModel = loginIno as XiaoMiLoginModel;
            if (loginModel == null)
                return OperateResult.Failed(null, string.Empty, "当前登录信息转换失败");

            var existUser = await _userRepo.FindByUserKeyInfoAsync(loginModel.UserPhone, StepFlyProviderType.XiaoMi);

            OperateResult operateResult = await Login(loginModel, existUser, cancellationToken);

            if (!operateResult.Succeeded)
                return operateResult;

            var result = operateResult.Information.PlayLoad! as XiaoMiLoginAPISuccessModel
                ?? throw new SoftlyMiCakeException($"转换{nameof(XiaoMiLoginAPISuccessModel)}返回结果出错");

            //保存当前用户的信息
            if (!result.AlreadyHasUser)
            {
                var user = StepFlyUser.Create(loginModel.UserPhone, loginModel.Password, result.UserId);
                user.SetProvider(StepFlyProviderType.XiaoMi);
                user.SetToken(result.Token, DateTime.Now.AddDays(30));
                user.SetClientInfo(null, result.DeviceId);

                await _userRepo.AddAsync(user);
                result.User = user;
            }
            else
            {
                existUser.SetToken(result.Token, DateTime.Now.AddDays(30));
                existUser.SetPassword(loginModel.Password);
                //保证设备ID不为空
                existUser.SetClientInfo(existUser.UserClientInfo, existUser.DeviceId);
                result.User = existUser;
            }

            return OperateResult.Success(HttpStatusCode.OK.ToString(), "登录成功", result);
        }

        public async Task<OperateResult> UpdateStepAsync(int stepNum, UpdateStepUser userInfo, CancellationToken cancellationToken = default)
        {
            try
            {
                CheckValue.NotNullOrWhiteSpace(userInfo.UserKeyInfo, "userInfo.UserKeyInfo");

                var user = await _userRepo.FindByUserKeyInfoAsync(userInfo.UserKeyInfo, StepFlyProviderType.XiaoMi) ??
                    throw new SoftlyMiCakeException("在修改步数的时候没有找到对应的用户信息");

                var httpClient = _httpClientFactory.CreateClient();
                var url = XiaoMiConfig.GetChangeStepUrl();

                var jsonOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var content = new StringContent(XiaoMiConfig.GetChangeStepRequestBody(user.UserSystemId, stepNum.ToString()), Encoding.UTF8, "application/x-www-form-urlencoded");
                content.Headers.Add("apptoken", user.TokenInfo);

                using var response = await httpClient.PostAsync(url, content, cancellationToken);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation(await content.ReadAsStringAsync());
                _logger.LogInformation(responseContent);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "修改步数失败", responseContent);
                }

                using var jsonDoc = JsonDocument.Parse(responseContent);
                var successCode = jsonDoc.RootElement.GetProperty("code").GetRawText();
                if (successCode.Equals("1"))
                    return OperateResult.Success(HttpStatusCode.OK.ToString(), "修改步数成功", responseContent);

                if (successCode.Equals("0"))
                    return OperateResult.Failed(null, HttpStatusCode.Unauthorized.ToString(), "修改步数失败，登录信息已经过期");

                return OperateResult.Success(HttpStatusCode.OK.ToString(), "修改步数失败", responseContent);
            }
            catch (Exception ex)
            {
                return OperateResult.Failed(ex, "尝试修改步数时产生错误", ex.Message);
            }
        }

        private async Task<OperateResult> Login(XiaoMiLoginModel loginInfo, StepFlyUser user, CancellationToken cancellationToken)
        {
            try
            {
                CheckValue.NotNullOrWhiteSpace(loginInfo.UserPhone, nameof(loginInfo.UserPhone));
                CheckValue.NotNullOrWhiteSpace(loginInfo.Password, nameof(loginInfo.Password));

                var httpClient = _httpClientFactory.CreateClient("noRedirect");

                //Step one : Get AccessToken
                var accessUrl = XiaoMiConfig.GetAccessUrl(loginInfo.UserPhone);

                var content = new StringContent(XiaoMiConfig.GetAccessRequestBody(loginInfo.UserPhone, loginInfo.Password), Encoding.UTF8, "application/x-www-form-urlencoded");
                //add important headers
                content.Headers.Add("hm-privacy-diagnostics", "false");
                content.Headers.Add("app_name", "com.xiaomi.hm.health");
                content.Headers.Add("hm-privacy-ceip", "true");
                content.Headers.Add("X-Request-Id", Guid.NewGuid().ToString());

                using var response = await httpClient.PostAsync(accessUrl, content, cancellationToken);
                if (response.StatusCode != HttpStatusCode.RedirectMethod)
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "登录失败", "尝试获取AccessToken时失败");

                var parms = HttpUtility.ParseQueryString(response.Headers.Location.Query);

                var accessToken = parms["access"];

                if (string.IsNullOrWhiteSpace(accessToken))
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "登录失败", "尝试获取AccessToken时失败");

                //Step two : Login to system
                string deviceId = user?.DeviceId ?? IdentityHelper.GetRandomDeviceId();
                var loginContent = new StringContent(XiaoMiConfig.GetLoginRequestBody(accessToken, HttpUtility.UrlEncode(deviceId, Encoding.UTF8)), Encoding.UTF8, "application/x-www-form-urlencoded");

                using var loginResponse = await httpClient.PostAsync(XiaoMiConfig.LoginUrl, loginContent, cancellationToken);
                loginResponse.EnsureSuccessStatusCode();

                var responseContent = await loginResponse.Content.ReadAsStringAsync();

                _logger.LogInformation(await loginContent.ReadAsStringAsync());
                _logger.LogInformation(responseContent);

                if (loginResponse.StatusCode != HttpStatusCode.OK)
                {
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "登录失败", responseContent);
                }

                //得到当前登录成功的用户信息
                var successModel = JsonSerializer.Deserialize<XiaoMiLoginSuccessModel>(responseContent, new JsonSerializerOptions() { IgnoreNullValues = true });

                XiaoMiLoginAPISuccessModel result = new XiaoMiLoginAPISuccessModel()
                {
                    AlreadyHasUser = user != null,
                    APIResponseData = responseContent,
                    DeviceId = deviceId,
                    Token = successModel.token_info.app_token,
                    UserId = successModel.token_info.user_id,
                };

                return OperateResult.Success(HttpStatusCode.OK.ToString(), "登录成功", result);
            }
            catch (Exception ex)
            {
                return OperateResult.Failed(ex, "尝试登录时发生错误", ex.Message);
            }
        }
    }

    //登录方法成功返回的结果
    public class XiaoMiLoginAPISuccessModel
    {
        public string Token { get; set; }

        public bool AlreadyHasUser { get; set; }

        public string DeviceId { get; set; }

        public string APIResponseData { get; set; }

        public string UserId { get; set; }

        public StepFlyUser User { get; set; }
    }
}
