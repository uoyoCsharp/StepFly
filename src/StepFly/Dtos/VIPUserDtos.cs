using System;

namespace StepFly.Dtos
{
    public class VIPUserDto
    {
        public bool IsVip { get; set; }

        public int Level { get; set; }

        public DateTime? ExpireTime { get; set; }
    }

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
