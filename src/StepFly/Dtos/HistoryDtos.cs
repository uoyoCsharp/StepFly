using System;

namespace StepFly.Dtos
{
    public class HistoryDto
    {
        public long Id { get; set; }

        public string UserKeyInfo { get; set; }

        public int StepNum { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
