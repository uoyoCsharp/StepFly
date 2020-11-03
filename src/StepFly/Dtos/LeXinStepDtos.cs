using System;
using System.ComponentModel.DataAnnotations;

namespace StepFly.Dtos
{
    /// <summary>
    /// 登录成功的Dto
    /// </summary>
    public class LoginResultDto
    {
        public Guid UserId { get; set; }

        public bool Success { get; set; }

        public string Msg { get; set; }

        public string Token { get; set; }

        public bool IsLockout { get; set; }

        public string Roles { get; set; }
    }

    /// <summary>
    /// 乐心使用验证码登录的dto
    /// </summary>
    public class LeXinLoginWithAuthCodeDto
    {
        [Phone]
        public string Phone { get; set; }

        public string Code { get; set; }
    }

    /// <summary>
    /// 乐心使用账号密码登录的dto
    /// </summary>
    public class LeXinLoginWithPasswordDto
    {
        [Phone]
        public string Phone { get; set; }

        public string Password { get; set; }
    }

    /// <summary>
    /// 乐心使用更改步数的dto
    /// </summary>
    public class ChangeLeXinStepDto
    {
        [Phone]
        public string Phone { get; set; }

        public int Step { get; set; }
    }

    /// <summary>
    /// 乐心更改步数结果的dto
    /// </summary>
    public class ChangeLeXinStepResultDto
    {
        public bool Success { get; set; }

        public string Code { get; set; }

        public string Msg { get; set; }
    }

    /// <summary>
    /// 乐心发送验证码的dto
    /// </summary>
    public class LeXinSendCodeDto
    {
        [Phone]
        public string Phone { get; set; }

        public string ImageCode { get; set; }
    }

    /// <summary>
    /// 乐心绑定的类型
    /// </summary>
    public class LeXinBindingType
    {
        public int userId { get; set; }

        public bool wechatBinding { get; set; }

        public bool firstBindingWechat { get; set; }

        public bool wechatServiceNoBinding { get; set; }

        public bool lifeSenseSportFollowing { get; set; }

        public bool qqBinding { get; set; }

        public bool taobaoBinding { get; set; }

        public bool antForestBinding { get; set; }

        public bool alispBinding { get; set; }

        public bool googleBinding { get; set; }

        public bool facebookBinding { get; set; }
    }
}
