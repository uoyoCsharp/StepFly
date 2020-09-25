using MiCake.Audit;
using MiCake.Core.Util;
using MiCake.DDD.Domain;
using System;
using System.Text;

namespace StepFly.Domain
{
    /// <summary>
    /// 登录系统的用户
    /// </summary>
    public class StepFlyUser : AggregateRoot<Guid>, IHasModificationTime
    {
        public StepFlyUser()
        {
            Provider = StepFlyProviderType.LeXin;
        }

        /// <summary>
        /// 用户所在的系统
        /// </summary>
        public StepFlyProviderType Provider { get; private set; }

        /// <summary>
        /// 用户的主要身份信息，比如手机号或者用户名
        /// </summary>
        public string UserKeyInfo { get; private set; }

        /// <summary>
        /// 登录的密码
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 用户在系统中的userid
        /// </summary>
        public string UserSystemId { get; private set; }

        /// <summary>
        /// 用户登录成功后的令牌信息
        /// </summary>
        public string TokenInfo { get; private set; }

        /// <summary>
        /// 令牌过期的时间
        /// </summary>
        public DateTime TokenExpireTime { get; private set; }

        /// <summary>
        /// 用户的设备信息
        /// </summary>
        public string UserClientInfo { get; private set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public string DeviceId { get; private set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; private set; }

        /// <summary>
        /// 账户是否被锁定
        /// </summary>
        public bool IsLockout { get; private set; }

        /// <summary>
        /// 一些附加信息
        /// </summary>
        public string AdditionalInfo { get; private set; }

        public DateTime? ModificationTime { get; set; }

        public static StepFlyUser Create(string userKeyInfo, string password, string userSystemId)
        {
            CheckValue.NotNullOrWhiteSpace(userKeyInfo, nameof(userKeyInfo));

            return new StepFlyUser()
            {
                UserKeyInfo = userKeyInfo,
                Password = password,
                UserSystemId = userSystemId,
                LoginTime = DateTime.Now
            };
        }

        /// <summary>
        /// 设置系统来源
        /// </summary>
        /// <param name="provider"></param>
        public void SetProvider(StepFlyProviderType provider)
           => Provider = provider;

        /// <summary>
        /// 设置token信息
        /// </summary>
        public void SetToken(string tokenInfo, DateTime tokenExpire)
        {
            CheckValue.NotNullOrWhiteSpace(tokenInfo, nameof(tokenInfo));

            TokenInfo = tokenInfo;
            TokenExpireTime = tokenExpire;
        }

        /// <summary>
        /// 设置用户的设备信息
        /// </summary>
        public void SetClientInfo(string clientInfo, string deviceId)
        {
            if (string.IsNullOrWhiteSpace(UserClientInfo) && string.IsNullOrWhiteSpace(clientInfo))
            {
                UserClientInfo = Guid.NewGuid().ToString().Replace("-", "");
            }
            else
            {
                UserClientInfo = clientInfo;
            }

            if (string.IsNullOrWhiteSpace(DeviceId) && string.IsNullOrWhiteSpace(deviceId))
            {
                DeviceId = $"M_868{GenerateRandomCode(12)}";
            }
            else
            {
                DeviceId = deviceId;
            }
        }

        public void SetAdditionalInfo(string info)
            => AdditionalInfo = info;


        /// <summary>
        /// 获取随机数，用于DeviceId
        /// </summary>
        private string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return result.ToString();
        }
    }

    /// <summary>
    /// 支持的系统种类
    /// </summary>
    public enum StepFlyProviderType
    {
        LeXin = 0,
    }
}


