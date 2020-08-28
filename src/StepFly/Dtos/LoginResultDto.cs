namespace StepFly.Dtos
{
    /// <summary>
    /// 登录的结果
    /// </summary>
    public class LoginResultDto
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }
    }
}
