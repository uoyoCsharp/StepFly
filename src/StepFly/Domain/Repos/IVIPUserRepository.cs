using MiCake.DDD.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Domain.Repos
{
    public interface IVIPUserRepository : IRepository<VIPUser, int>
    {
        Task<VIPUser> FindByUserId(Guid userId, CancellationToken cancellationToken = default);
    }
}
