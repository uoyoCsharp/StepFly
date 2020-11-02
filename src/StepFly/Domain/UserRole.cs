using MiCake.Audit;
using MiCake.Core.Util;
using MiCake.DDD.Domain;
using System;

namespace StepFly.Domain
{
    public class UserRole : AggregateRoot, IHasCreationTime
    {
        public Guid UserId { get; private set; }

        public string RoleName { get; private set; }

        public DateTime CreationTime { get; set; }

        public UserRole(Guid userId, string roleName)
        {
            CheckValue.NotNull(userId, nameof(userId));
            CheckValue.NotNullOrWhiteSpace(roleName, nameof(roleName));

            UserId = userId;
            RoleName = roleName;
        }

        public UserRole()
        {
        }

        public static UserRole Create(Guid userId, string roleName)
        {
            return new UserRole(userId, roleName);
        }
    }
}
