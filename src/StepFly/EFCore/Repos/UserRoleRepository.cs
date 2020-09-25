using MiCake.EntityFrameworkCore.Repository;
using StepFly.Domain;
using StepFly.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.EFCore.Repos
{
    public class UserRoleRepository : EFRepository<StepFlyDbContext, UserRole, int>, IUserRoleRepository
    {
        public UserRoleRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<List<UserRole>> GetUserRoles(Guid userId, CancellationTokenSource cancellationToken = null)
        {
            return Task.FromResult(DbSet.Where(s => s.UserId.Equals(userId)).ToList());
        }
    }
}
