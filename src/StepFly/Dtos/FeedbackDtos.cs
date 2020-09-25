namespace StepFly.Dtos
{
    public class FeedbackDto
    {
        public int Id { get; set; }

        public string UserKey { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Tag { get; set; }
    }

    public class AddFeedbackDto
    {
        public string UserKey { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Tag { get; set; }
    }
}
