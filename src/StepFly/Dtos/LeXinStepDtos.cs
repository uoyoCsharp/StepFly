using System.ComponentModel.DataAnnotations;

namespace StepFly.Dtos
{
    /// <summary>
    /// 登录成功的Dto
    /// </summary>
    public class LoginResultDto
    {
        public bool Success { get; set; }

        public string Msg { get; set; }

        public string Token { get; set; }
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
}
