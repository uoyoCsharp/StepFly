using MiCake.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using StepFly.Dtos;
using StepFly.Services.LeXin;
using StepFly.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    public class LeXinController : ApplicationController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LeXinController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetValidateCodeImg([Phone] string phone)
        {
            var httpClient = _httpClientFactory.CreateClient();

            using var res = await httpClient.GetAsync(LeXinConfig.GetValidateCodeImgUrl(phone), AbortToken);
            res.EnsureSuccessStatusCode();

            var fileBytes = await res.Content.ReadAsByteArrayAsync();
            return File(fileBytes, "image/jpeg");
        }

        [HttpPost]
        public async Task<bool> SendPhoneCode(LeXinSendCodeDto dto)
        {
            var requestId = Guid.NewGuid().ToString().Replace("-", "");
            var tokenId = Guid.NewGuid().ToString().Replace("-", "");
            var url = string.Format(LeXinConfig.SendPhoneCodeUrl, requestId, tokenId, DateTimeHelper.GetTimeStamp().ToString());
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var content = new StringContent($"{{\"code\":\"{dto.ImageCode}\",\"mobile\":\"{dto.Phone}\"}}", Encoding.UTF8, "application/json");
                using var res = await httpClient.PostAsync(url, content, AbortToken);
                res.EnsureSuccessStatusCode();

                var apiResult = JsonSerializer.Deserialize<LeXinHttpResponse>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (apiResult.Code != 200)
                {
                    throw new SoftlyMiCakeException(apiResult.Msg);
                }

                return true;
            }
            catch
            {
                throw new SoftlyMiCakeException("访问乐心api出错");
            }
        }
    }
}
