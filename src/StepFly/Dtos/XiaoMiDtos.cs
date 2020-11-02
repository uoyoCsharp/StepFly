using System.ComponentModel.DataAnnotations;

namespace StepFly.Dtos
{
    /// <summary>
    /// 小米使用账号密码登录的dto
    /// </summary>
    public class XiaoMiLoginWithPasswordDto
    {
        [Phone]
        public string Phone { get; set; }

        public string Password { get; set; }
    }

    /// <summary>
    /// 小米使用更改步数的dto
    /// </summary>
    public class ChangeXiaoMiStepDto
    {
        [Phone]
        public string Phone { get; set; }

        public int Step { get; set; }
    }

    /// <summary>
    /// 小米更改步数结果的dto
    /// </summary>
    public class ChangeXiaoMiStepResultDto
    {
        public bool Success { get; set; }

        public string Code { get; set; }

        public string Msg { get; set; }
    }
}
