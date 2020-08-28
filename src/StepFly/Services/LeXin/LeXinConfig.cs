namespace StepFly.Services.LeXin
{
    internal class LeXinConfig
    {
        public const string UploadInfoUrl = @"https://sports.lifesense.com/sport_service/sport/sport/uploadMobileStepV2?country=%E4%B8%AD%E5%9B%BD&city=%E6%88%90%E9%83%BD&cityCode=510100&timezone=Asia%2FShanghai&latitude=30.650636&os_country=CN&channel=qq&language=zh&openudid=&platform=android&province=%E5%9B%9B%E5%B7%9D%E7%9C%81&appType=6&requestId={0}&countryCode=&systemType=2&longitude=104.200846&devicemodel={1}&area=CN&screenwidth=1080&os_langs=zh&provinceCode=510000&promotion_channel=qq&version=4.6.7&areaCode=510112&requestToken={2}&network_type=wifi&osversion=9&screenheight=2040&ts={3}";

        public const string LoginUseCodeUrl = @"https://sports.lifesense.com/sessions_service/loginByAuth?country=%E4%B8%AD%E5%9B%BD&screenWidth=1080&city=%E6%88%90%E9%83%BD&cityCode=510100&timezone=Asia%2FShanghai&latitude=30.650646&os_country=CN&channel=qq&language=zh&openudid=&platform=android&province=%E5%9B%9B%E5%B7%9D%E7%9C%81&appType=6&requestId={0}&countryCode=&systemType=2&longitude=104.200769&devicemodel={1}&area=CN&screenwidth=1080&os_langs=zh&clientId={4}&provinceCode=510000&screenHeight=2040&promotion_channel=qq&version=4.6.7&areaCode=510112&requestToken={2}&network_type=wifi&osversion=9&screenheight=2040&ts={3}";

        public const string DefaultDeviceModel = "Mi10";
    }
}
