namespace StepFly.Dtos
{
    public class NoticeDto
    {
        public int Id { get; set; }

        /// <summary>
        /// 公告内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }
    }
}
