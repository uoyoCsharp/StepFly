using MiCake.EntityFrameworkCore.Repository;
using StepFly.Domain;
using System;
using System.Linq;
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
    }
}
