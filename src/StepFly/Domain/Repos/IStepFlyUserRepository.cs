using MiCake.DDD.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Domain
{
    public interface IStepFlyUserRepository : IRepository<StepFlyUser, Guid>
    {
        Task<StepFlyUser> FindByUserKeyInfoAsync(string userKey, StepFlyProviderType providerType = StepFlyProviderType.LeXin);

        Task<List<StepFlyUser>> GetUserList(int pageIndex, int pageNum, CancellationToken cancellationToken = default);
    }
}
