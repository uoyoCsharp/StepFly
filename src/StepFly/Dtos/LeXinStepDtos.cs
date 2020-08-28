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
}
