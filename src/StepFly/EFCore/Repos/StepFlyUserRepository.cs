using MiCake.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
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

        public async Task<long> GetCountByType(StepFlyProviderType type, CancellationToken cancellationToken = default)
        {
            return await DbSet.CountAsync(s => s.Provider == type);
        }

        public Task<List<StepFlyUser>> GetUserList(int pageIndex, int pageNum, StepFlyProviderType providerType, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(DbSet.Where(s=> s.Provider == providerType)
                                        .OrderByDescending(s => s.LoginTime)
                                        .Skip((pageIndex - 1) * pageNum)
                                        .Take(pageNum)
                                        .ToList());
        }
    }
}
