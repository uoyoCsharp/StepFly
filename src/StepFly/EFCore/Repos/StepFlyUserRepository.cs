using MiCake.EntityFrameworkCore.Repository;
using StepFly.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.EFCore.Repos
{
    public class StepFlyUserRepository : EFRepository<StepFlyDbContext, StepFlyUser, Guid>, IStepFlyUserRepository
    {
        public StepFlyUserRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<StepFlyUser> FindByUserKeyInfoAsync(string userKey, StepFlyProviderType providerType = StepFlyProviderType.LeXin)
        {
            return Task.FromResult(DbSet.FirstOrDefault(s => s.UserKeyInfo.Equals(userKey) && s.Provider == providerType));
        }

        public Task<List<StepFlyUser>> GetUserList(int pageIndex, int pageNum, CancellationToken cancellationToken = default)
        {
            if (pageIndex < 1)
                throw new ArgumentException("page页数不正确");
            return Task.FromResult(DbSet.OrderByDescending(s => s.LoginTime).Skip((pageIndex - 1) * pageNum).Take(pageNum).ToList());
        }
    }
}
