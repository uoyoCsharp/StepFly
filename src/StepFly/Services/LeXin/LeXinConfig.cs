﻿namespace StepFly.Services.LeXin
{
    internal class LeXinConfig
    {
        public const string UploadInfoUrl = @"https://sports.lifesense.com/sport_service/sport/sport/uploadMobileStepV2?country=%E4%B8%AD%E5%9B%BD&city=%E6%88%90%E9%83%BD&cityCode=510100&timezone=Asia%2FShanghai&latitude=30.650636&os_country=CN&channel=qq&language=zh&openudid=&platform=android&province=%E5%9B%9B%E5%B7%9D%E7%9C%81&appType=6&requestId={0}&countryCode=&systemType=2&longitude=104.200846&devicemodel={1}&area=CN&screenwidth=1080&os_langs=zh&provinceCode=510000&promotion_channel=qq&version=4.6.7&areaCode=510112&requestToken={2}&network_type=wifi&osversion=9&screenheight=2040&ts={3}";

        /// <summary>
        /// 乐心使用验证码登录的URL
        /// </summary>
        public const string LoginUseCodeUrl = @"https://sports.lifesense.com/sessions_service/loginByAuth?country=%E4%B8%AD%E5%9B%BD&screenWidth=1080&city=%E6%88%90%E9%83%BD&cityCode=510100&timezone=Asia%2FShanghai&latitude=30.650646&os_country=CN&channel=qq&language=zh&openudid=&platform=android&province=%E5%9B%9B%E5%B7%9D%E7%9C%81&appType=6&requestId={0}&countryCode=&systemType=2&longitude=104.200769&devicemodel={1}&area=CN&screenwidth=1080&os_langs=zh&clientId={4}&provinceCode=510000&screenHeight=2040&promotion_channel=qq&version=4.6.7&areaCode=510112&requestToken={2}&network_type=wifi&osversion=9&screenheight=2040&ts={3}";

        /// <summary>
        /// 乐心使用账号密码登录的URL
        /// </summary>
        public const string LoginWithPassword = @"https://sports.lifesense.com:443/sessions_service/login?country=&screenWidth=1080&city=&cityCode=&timezone=Asia%2FShanghai&latitude=&os_country=CN&channel=qq&language=zh&roleType=0&openudid=&platform=android&province=&appType=6&requestId={0}&countryCode=&systemType=2&longitude=&devicemodel={1}&area=CN&screenwidth=1080&os_langs=zh&provinceCode=&screenHeight=2040&promotion_channel=qq&rnd=e325d174&version=4.6.7&areaCode=&requestToken={2}&network_type=4G&osversion=9&screenheight=2040&ts={3}";

        public const string DefaultDeviceModel = "Mi10";

        /// <summary>
        /// 乐心发送验证码的url
        /// 参数0:client ID,参数1:token id,参数2:time stamp.
        /// </summary>
        public const string SendPhoneCodeUrl = @"https://sports.lifesense.com/sms_service/verify/sendCodeWithOptionalValidate?country=%E4%B8%AD%E5%9B%BD&screenWidth=1080&city=%E6%88%90%E9%83%BD&cityCode=510100&timezone=Asia%2FShanghai&latitude=30.65064&os_country=CN&channel=qq&language=zh&openudid=&platform=android&province=%E5%9B%9B%E5%B7%9D%E7%9C%81&appType=6&requestId=1000&countryCode=&systemType=2&longitude=104.200778&devicemodel=NX619J&area=CN&screenwidth=1080&os_langs=zh&clientId={0}&provinceCode=510000&screenHeight=2040&promotion_channel=qq&rnd=10d86c0d&sessionId=nosession&version=4.6.7&areaCode=510112&requestToken={1}&network_type=wifi&osversion=9&screenheight=2040&ts={2}";

        /// <summary>
        ///  乐心获取绑定设备种类的url
        ///  参数0：requestId
        /// </summary>
        public const string GetBindTypeUrl = @"https://sports.lifesense.com/user_service/myThirdpartyBindings?appType=6&language=zh&screenWidth=1080&screenHeight=2040&requestId={0}&systemType=2&os_country=CN&os_langs=zh&channel=qq&city=%E6%88%90%E9%83%BD&cityCode=510100&province=%E5%9B%9B%E5%B7%9D%E7%9C%81&provinceCode=510000&areaCode=510105&country=%E4%B8%AD%E5%9B%BD&countryCode=&version=4.6.7";

        /// <summary>
        /// 得到乐心获取验证码图片的url
        /// </summary>
        public static string GetValidateCodeImgUrl(string phone)
        {
            return $"https://sports.lifesense.com/sms_service/verify/getValidateCode?requestId=1000&sessionId=nosession&mobile={phone}";
        }
    }
}
