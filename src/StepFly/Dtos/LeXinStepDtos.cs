using System.ComponentModel.DataAnnotations;

namespace StepFly.Dtos
{
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
    /// 乐心使用验证码登录的dto
    /// </summary>
    public class ChangeLeXinStepDto
    {
        [Phone]
        public string Phone { get; set; }

        public int Step { get; set; }
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
