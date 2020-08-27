using StepFly.Core;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Services
{
    public class LeXinStepFlyService : IStepFlyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LeXinStepFlyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<OperateResult> UpdateStepAsync(int stepNum, CancellationToken cancellationToken = default)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var url = LeXinConfig.UploadInfoUrl + "myTimeStamp";

            var content = new StringContent("");
            content.Headers.Add("Cookie", "myLeXinCookie");

            try
            {
                using var response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return OperateResult.Failed(null, response.StatusCode.ToString(), "修改步数失败", responseContent);
                }

                return OperateResult.Success(HttpStatusCode.OK.ToString(), "修改步数成功", responseContent);
            }
            catch (Exception ex)
            {
                return OperateResult.Failed(ex);
            }
        }


        class LeXinConfig
        {
            public const string UploadInfoUrl = @"https://sports.lifesense.com/sport_service/sport/sport/uploadMobileStepV2?
                                                 country=%E4%B8%AD%E5%9B%BD&city=%E6%88%90%E9%83%BD&cityCode=510100&timezone=Asia%2FShanghai&latitude=30.650636
                                                 &os_country=CN&channel=qq&language=zh&openudid=&platform=android&province=%E5%9B%9B%E5%B7%9D%E7%9C%81&appType=6
                                                 &requestId=32e634d71ffd45b4adf48a8dbe14d4d6&countryCode=&systemType=2&longitude=104.200846&devicemodel=NX619J
                                                 &area=CN&screenwidth=1080&os_langs=zh&provinceCode=510000&promotion_channel=qq&rnd=97b2e588&version=4.6.7&areaCode=510112
                                                 &requestToken=d485e665c739641e8448f3364a57f6ed&network_type=wifi&osversion=9&screenheight=2040&ts=";
        }
    }
}
