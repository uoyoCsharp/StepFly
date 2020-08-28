using MiCake.DDD.Domain;
using System;
using System.Threading.Tasks;

namespace StepFly.Domain
{
    public interface IStepFlyUserRepository : IRepository<StepFlyUser, Guid>
    {
        Task<StepFlyUser> FindByUserKeyInfoAsync(string userKey, StepFlyProviderType providerType = StepFlyProviderType.LeXin);
    }
}
