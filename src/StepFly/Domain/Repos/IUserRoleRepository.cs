using MiCake.DDD.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Domain.Repos
{
    public interface IUserRoleRepository : IRepository<UserRole, int>
    {
        /// <summary>
        /// 获取用户所拥有的角色项
        /// </summary>
        Task<List<UserRole>> GetUserRoles(Guid userId, CancellationToken cancellationToken = default);
    }
}
