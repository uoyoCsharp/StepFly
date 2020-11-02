using System;

namespace StepFly.Dtos
{
    public class ActiveVIPDto
    {
        public Guid UserId { get; set; }

        public int EffectDay { get; set; }
    }

    public class StopVIPDto
    {
        public Guid UserId { get; set; }
    }
}
