using MiCake.Core.Util;
using StepFly.Utils;
using System;

namespace StepFly.Services.XiaoMi
{
    internal class XiaoMiConfig
    {
        /// <summary>
        /// 获取Access Token的url，用于下一步登录
        /// </summary>
        public const string AccessTokenUrl = "https://api-user.huami.com/registrations/%2B86{0}/tokens";

        /// <summary>
        /// 用于登录的URL
        /// </summary>
        public const string LoginUrl = "https://account.huami.com/v2/client/login";

        /// <summary>
        /// 用于重新登录的URL
        /// </summary>
        public const string ReLoginUrl = "https://account-cn2.huami.com/v1/client/re_login";

        /// <summary>
        /// 用于修改步数的Url
        /// </summary>
        public const string ChangeStepUrl = "https://api-mifit-cn2.huami.com/v1/data/band_data.json?&t={0}";

        /// <summary>
        /// 根据传入的电话号码获取AccessToken Url
        /// </summary>
        public static string GetAccessUrl(string phone)
        {
            CheckValue.NotNullOrEmpty(phone, nameof(phone));
            return string.Format(AccessTokenUrl, phone);
        }

        /// <summary>
        /// 获取交换AccessToken的请求body
        /// </summary>
        public static string GetAccessRequestBody(string phone, string pwd)
        {
            return $"phone_number=%2B86{phone}&password={pwd}&state=REDIRECTION&client_id=HuaMi&country_code=CN&token=access&token=refresh&region=cn-northwest-1&redirect_uri=https%3A%2F%2Fs3-us-west-2.amazonaws.com%2Fhm-registration%2Fsuccesssignin.htmlhm-privacy-diagnostics: false";
        }

        public static string GetLoginRequestBody(string token, string deciceId)
        {
            return $"app_name=com.xiaomi.hm.health&country_code=CN&code={token}&device_id={deciceId}&device_model=android_phone&app_version=4.6.5&grant_type=access_token&allow_registration=false&dn=account.huami.com%2Capi-user.huami.com%2Capi-watch.huami.com%2Cauth.huami.com%2Capi-analytics.huami.com%2Capp-analytics.huami.com%2Capi-mifit.huami.com&third_name=huami_phone&source=com.xiaomi.hm.health%3A4.6.5%3A50363&lang=zh&";
        }

        /// <summary>
        /// 获取重新登录的请求body
        /// </summary>
        /// <param name="login_token"></param>
        /// <param name="deciceId"></param>
        /// <returns></returns>
        public static string GetReLoginRequestBody(string login_token, string deciceId)
        {
            return $"app_name=com.xiaomi.hm.health&device_id={deciceId}&device_model=android_phone&os_version=v0.6.8&login_token={login_token}&device_id_type=mac&source=com.xiaomi.hm.health%3A4.6.5%3A50363&lang=zh&";
        }

        /// <summary>
        /// 根据当前的时间戳来返回修改步数的URL
        /// </summary>
        /// <returns></returns>
        public static string GetChangeStepUrl()
        {
            return string.Format(ChangeStepUrl, DateTimeHelper.GetTimeStamp(DateTime.Now).ToString());
        }

        public static string GetChangeStepRequestBody(string userId, string stepNum)
        {
            var lastSyncTime = DateTimeHelper.GetTimeStamp(DateTime.Now.AddHours(-3.5));
            var lastTime = DateTimeHelper.GetTimeStamp(DateTime.Now.AddHours(-2.5));
            var completeTime = DateTimeHelper.GetTimeStamp(DateTime.Now.AddHours(-1));
            return $@"userid={userId}&last_sync_data_time={lastSyncTime}&device_type=0&last_deviceid=DA932FFFFE8816E7&data_json=%5B%7B%22data_hr%22:%22%5C/%5C/%5C/%5C/%5C/%5C/9L%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/Vv%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/0v%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/9e%5C/%5C/%5C/%5C/%5C/0n%5C/a%5C/%5C/%5C/S%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/0b%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/1FK%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/R%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/9PTFFpaf9L%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/R%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/0j%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/9K%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/Ov%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/zf%5C/%5C/%5C/86%5C/zr%5C/Ov88%5C/zf%5C/Pf%5C/%5C/%5C/0v%5C/S%5C/8%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/Sf%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/z3%5C/%5C/%5C/%5C/%5C/%5C/0r%5C/Ov%5C/%5C/%5C/%5C/%5C/%5C/S%5C/9L%5C/zb%5C/Sf9K%5C/0v%5C/Rf9H%5C/zj%5C/Sf9K%5C/0%5C/%5C/N%5C/%5C/%5C/%5C/0D%5C/Sf83%5C/zr%5C/Pf9M%5C/0v%5C/Ov9e%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/S%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/zv%5C/%5C/z7%5C/O%5C/83%5C/zv%5C/N%5C/83%5C/zr%5C/N%5C/86%5C/z%5C/%5C/Nv83%5C/zn%5C/Xv84%5C/zr%5C/PP84%5C/zj%5C/N%5C/9e%5C/zr%5C/N%5C/89%5C/03%5C/P%5C/89%5C/z3%5C/Q%5C/9N%5C/0v%5C/Tv9C%5C/0H%5C/Of9D%5C/zz%5C/Of88%5C/z%5C/%5C/PP9A%5C/zr%5C/N%5C/86%5C/zz%5C/Nv87%5C/0D%5C/Ov84%5C/0v%5C/O%5C/84%5C/zf%5C/MP83%5C/zH%5C/Nv83%5C/zf%5C/N%5C/84%5C/zf%5C/Of82%5C/zf%5C/OP83%5C/zb%5C/Mv81%5C/zX%5C/R%5C/9L%5C/0v%5C/O%5C/9I%5C/0T%5C/S%5C/9A%5C/zn%5C/Pf89%5C/zn%5C/Nf9K%5C/07%5C/N%5C/83%5C/zn%5C/Nv83%5C/zv%5C/O%5C/9A%5C/0H%5C/Of8%5C/%5C/zj%5C/PP83%5C/zj%5C/S%5C/87%5C/zj%5C/Nv84%5C/zf%5C/Of83%5C/zf%5C/Of83%5C/zb%5C/Nv9L%5C/zj%5C/Nv82%5C/zb%5C/N%5C/85%5C/zf%5C/N%5C/9J%5C/zf%5C/Nv83%5C/zj%5C/Nv84%5C/0r%5C/Sv83%5C/zf%5C/MP%5C/%5C/%5C/zb%5C/Mv82%5C/zb%5C/Of85%5C/z7%5C/Nv8%5C/%5C/0r%5C/S%5C/85%5C/0H%5C/QP9B%5C/0D%5C/Nf89%5C/zj%5C/Ov83%5C/zv%5C/Nv8%5C/%5C/0f%5C/Sv9O%5C/0ZeXv%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/1X%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/9B%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/TP%5C/%5C/%5C/1b%5C/%5C/%5C/%5C/%5C/%5C/0%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/9N%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%5C/v7+%22,%22date%22:%22{DateTime.Now.ToString("yyyy-MM-dd")}%22,%22data%22:%5B%7B%22start%22:0,%22stop%22:1439,%22value%22:%22UA8AUBQAUAwAUBoAUAEAYCcAUBkAUB4AUBgAUCAAUAEAUBkAUAwAYAsAYB8AYB0AYBgAYCoAYBgAYB4AUCcAUBsAUB8AUBwAUBIAYBkAYB8AUBoAUBMAUCEAUCIAYBYAUBwAUCAAUBgAUCAAUBcAYBsAYCUAATIPYD0KECQAYDMAYB0AYAsAYCAAYDwAYCIAYB0AYBcAYCQAYB0AYBAAYCMAYAoAYCIAYCEAYCYAYBsAYBUAYAYAYCIAYCMAUB0AUCAAUBYAUCoAUBEAUC8AUB0AUBYAUDMAUDoAUBkAUC0AUBQAUBwAUA0AUBsAUAoAUCEAUBYAUAwAUB4AUAwAUCcAUCYAUCwKYDUAAUUlEC8IYEMAYEgAYDoAYBAAUAMAUBkAWgAAWgAAWgAAWgAAWgAAUAgAWgAAUBAAUAQAUA4AUA8AUAkAUAIAUAYAUAcAUAIAWgAAUAQAUAkAUAEAUBkAUCUAWgAAUAYAUBEAWgAAUBYAWgAAUAYAWgAAWgAAWgAAWgAAUBcAUAcAWgAAUBUAUAoAUAIAWgAAUAQAUAYAUCgAWgAAUAgAWgAAWgAAUAwAWwAAXCMAUBQAWwAAUAIAWgAAWgAAWgAAWgAAWgAAWgAAWgAAWgAAWREAWQIAUAMAWSEAUDoAUDIAUB8AUCEAUC4AXB4AUA4AWgAAUBIAUA8AUBAAUCUAUCIAUAMAUAEAUAsAUAMAUCwAUBYAWgAAWgAAWgAAWgAAWgAAWgAAUAYAWgAAWgAAWgAAUAYAWwAAWgAAUAYAXAQAUAMAUBsAUBcAUCAAWwAAWgAAWgAAWgAAWgAAUBgAUB4AWgAAUAcAUAwAWQIAWQkAUAEAUAIAWgAAUAoAWgAAUAYAUB0AWgAAWgAAUAkAWgAAWSwAUBIAWgAAUC4AWSYAWgAAUAYAUAoAUAkAUAIAUAcAWgAAUAEAUBEAUBgAUBcAWRYAUA0AWSgAUB4AUDQAUBoAXA4AUA8AUBwAUA8AUA4AUA4AWgAAUAIAUCMAWgAAUCwAUBgAUAYAUAAAUAAAUAAAUAAAUAAAUAAAUAAAUAAAUAAAWwAAUAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAeSEAeQ8AcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcBcAcAAAcAAAcCYOcBUAUAAAUAAAUAAAUAAAUAUAUAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcCgAeQAAcAAAcAAAcAAAcAAAcAAAcAYAcAAAcBgAeQAAcAAAcAAAegAAegAAcAAAcAcAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcCkAeQAAcAcAcAAAcAAAcAwAcAAAcAAAcAIAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcCIAeQAAcAAAcAAAcAAAcAAAcAAAeRwAeQAAWgAAUAAAUAAAUAAAUAAAUAAAcAAAcAAAcBoAeScAeQAAegAAcBkAeQAAUAAAUAAAUAAAUAAAUAAAUAAAcAAAcAAAcAAAcAAAcAAAcAAAegAAegAAcAAAcAAAcBgAeQAAcAAAcAAAcAAAcAAAcAAAcAkAegAAegAAcAcAcAAAcAcAcAAAcAAAcAAAcAAAcA8AeQAAcAAAcAAAeRQAcAwAUAAAUAAAUAAAUAAAUAAAUAAAcAAAcBEAcA0AcAAAWQsAUAAAUAAAUAAAUAAAUAAAcAAAcAoAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAYAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcBYAegAAcAAAcAAAegAAcAcAcAAAcAAAcAAAcAAAcAAAeRkAegAAegAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAEAcAAAcAAAcAAAcAUAcAQAcAAAcBIAeQAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcBsAcAAAcAAAcBcAeQAAUAAAUAAAUAAAUAAAUAAAUBQAcBYAUAAAUAAAUAoAWRYAWTQAWQAAUAAAUAAAUAAAcAAAcAAAcAAAcAAAcAAAcAMAcAAAcAQAcAAAcAAAcAAAcDMAeSIAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcAAAcBQAeQwAcAAAcAAAcAAAcAMAcAAAeSoAcA8AcDMAcAYAeQoAcAwAcFQAcEMAeVIAaTYAbBcNYAsAYBIAYAIAYAIAYBUAYCwAYBMAYDYAYCkAYDcAUCoAUCcAUAUAUBAAWgAAYBoAYBcAYCgAUAMAUAYAUBYAUA4AUBgAUAgAUAgAUAsAUAsAUA4AUAMAUAYAUAQAUBIAASsSUDAAUDAAUBAAYAYAUBAAUAUAUCAAUBoAUCAAUBAAUAoAYAIAUAQAUAgAUCcAUAsAUCIAUCUAUAoAUA4AUB8AUBkAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAAfgAA%22,%22tz%22:32,%22did%22:%22DA932FFFFE8816E7%22,%22src%22:24%7D%5D,%22summary%22:%22%7B%5C%22v%5C%22:6,%5C%22slp%5C%22:%7B%5C%22st%5C%22:{lastTime},%5C%22ed%5C%22:{completeTime},%5C%22dp%5C%22:39,%5C%22lt%5C%22:294,%5C%22wk%5C%22:0,%5C%22usrSt%5C%22:-1440,%5C%22usrEd%5C%22:-1440,%5C%22wc%5C%22:0,%5C%22is%5C%22:169,%5C%22lb%5C%22:10,%5C%22to%5C%22:23,%5C%22dt%5C%22:0,%5C%22rhr%5C%22:58,%5C%22ss%5C%22:69,%5C%22stage%5C%22:%5B%7B%5C%22start%5C%22:1698,%5C%22stop%5C%22:1711,%5C%22mode%5C%22:4%7D,%7B%5C%22start%5C%22:1712,%5C%22stop%5C%22:1728,%5C%22mode%5C%22:5%7D,%7B%5C%22start%5C%22:1729,%5C%22stop%5C%22:1818,%5C%22mode%5C%22:4%7D,%7B%5C%22start%5C%22:1819,%5C%22stop%5C%22:1832,%5C%22mode%5C%22:5%7D,%7B%5C%22start%5C%22:1833,%5C%22stop%5C%22:1920,%5C%22mode%5C%22:4%7D,%7B%5C%22start%5C%22:1921,%5C%22stop%5C%22:1928,%5C%22mode%5C%22:5%7D,%7B%5C%22start%5C%22:1929,%5C%22stop%5C%22:2030,%5C%22mode%5C%22:4%7D%5D%7D,%5C%22stp%5C%22:%7B%5C%22ttl%5C%22:{stepNum},%5C%22dis%5C%22:82,%5C%22cal%5C%22:5,%5C%22wk%5C%22:7,%5C%22rn%5C%22:0,%5C%22runDist%5C%22:23,%5C%22runCal%5C%22:3%7D,%5C%22goal%5C%22:8000,%5C%22tz%5C%22:%5C%2228800%5C%22,%5C%22sn%5C%22:%5C%22e716882f93da%5C%22%7D%22,%22source%22:24,%22type%22:0%7D%5D";
        }
    }
}
