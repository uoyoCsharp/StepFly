using MiCake.Core.Util;
using StepFly.Utils;
using System;
using System.Collections.Generic;

namespace StepFly.Services.LeXin
{
    /// <summary>
    /// 乐心的登陆Model
    /// </summary>
    public class LeXinLoginModel
    {
        public LeXinLoginType Type { get; set; }

        public LeXinAuthCodeLoginModel AuthCodeLoginInfo { get; set; }
    }

    /// <summary>
    /// 验证码登录的模型
    /// {"clientId":"5ef5383c646a451b8eb309d55364ecb0","authCode":"351308","appType":"6","loginName":"PhoneNo"}
    /// </summary>
    public class LeXinAuthCodeLoginModel
    {
        public string ClientId { get; set; }

        public string AuthCode { get; set; }

        public string AppType { get; set; } = "6";

        public string LoginName { get; set; }

        public static LeXinAuthCodeLoginModel Create(string phone, string code)
            => new LeXinAuthCodeLoginModel() { LoginName = phone, AuthCode = code };

        public void SetClientId(string clientId)
            => ClientId = clientId;
    }

    /// <summary>
    /// 登录乐心健康的类型
    /// </summary>
    public enum LeXinLoginType
    {
        /// <summary>
        /// 验证码
        /// </summary>
        AuthCode = 0,

        /// <summary>
        /// QQ
        /// </summary>
        QQ = 1,

        /// <summary>
        /// 微信
        /// </summary>
        WeChat = 2
    }

    /// <summary>
    /// 修改步数Model
    /// </summary>
    public class LeXinStepItem
    {
        public int Active { get; set; } = 1;

        public double Calories { get; set; }

        public string Created { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public int DataSource { get; set; } = 2;

        public string DayMeasurementTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        public string DeviceId { get; set; }

        public int Distance { get; set; }

        public string Id { get; set; }

        public int IsUpload { get; set; } = 0;

        public string MeasurementTime => Created;

        public int Priority { get; set; } = 0;

        public int Step { get; set; }

        public int Type { get; set; } = 2;

        public long Updated => DateTimeHelper.GetTimeStamp(TimeStampType.ThirteenLength);

        public string UserId { get; set; }

        public int ExerciseTime { get; set; } = 0;

        public void SetStep(int step)
        {
            if (step > 100000)
                throw new ArgumentException("步数不能超过10w");

            Step = step;
            Calories = Math.Round(step / 45.0, 2);
            Distance = (int)(step / 1.5);
        }

        public void SetUserId(string userId)
        {
            CheckValue.NotNullOrWhiteSpace(userId, "userId");

            UserId = userId;
        }

        public void SetId(string id)
        {
            CheckValue.NotNullOrWhiteSpace(id, "id");

            Id = id;
        }

        public void SetDeviceId(string deviceId)
        {
            CheckValue.NotNullOrWhiteSpace(deviceId, "deviceId");

            DeviceId = deviceId;
        }
    }

    /// <summary>
    /// 乐心用于修改步数的post模型
    /// </summary>
    public class LeXinUpdateStepModel
    {
        public List<LeXinStepItem> List { get; set; }
    }

    /// <summary>
    /// 乐心的http返回格式
    /// </summary>
    public class LeXinHttpResponse
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public string Data { get; set; }
    }

    /// <summary>
    /// 乐心用户登录成功后的Model
    /// </summary>
    public class LeXinUserLoginSuccessModel
    {
        public bool Exist { get; set; }

        public bool HasMobile { get; set; }

        public string UserId { get; set; }

        public string AccessToken { get; set; }

        public long ExpireAt { get; set; }

        public int UserType { get; set; }

        public bool NeedInfo { get; set; }
    }
}
