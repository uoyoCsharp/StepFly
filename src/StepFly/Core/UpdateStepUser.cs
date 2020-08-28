using StepFly.Domain;

namespace StepFly.Core
{
    /// <summary>
    /// 需要更改步数的用户信息
    /// </summary>
    public class UpdateStepUser
    {
        public StepFlyProviderType Provider { get; set; }

        public string UserKeyInfo { get; set; }

        public static UpdateStepUser Create(string keyInfo)
        {
            return new UpdateStepUser()
            {
                UserKeyInfo = keyInfo,
                Provider = StepFlyProviderType.LeXin
            };
        }
    }
}
