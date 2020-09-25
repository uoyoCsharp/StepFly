using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StepFly.Dtos
{
    public class FeedbackDto
    {
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
