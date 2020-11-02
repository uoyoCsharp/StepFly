using System;

namespace StepFly.Dtos
{
    public class StepFlyUserDto
    {
        public string UserKeyInfo { get; set; }

        public string UserSystemId { get; set; }

        public DateTime LoginTime { get; set; }

        public bool IsLockout { get; set; }

        public DateTime? ModificationTime { get; set; }
    }

    public class PromotedToAdminDto
    {
        public Guid UserId { get; set; }

        public string UserKeyInfo { get; set; }

        public int Type { get; set; }
    }
}
