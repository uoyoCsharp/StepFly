using MiCake.DDD.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Domain
{
    public interface IHomeConfigRepository : IRepository<HomeConfig, int>
    {
        Task<HomeConfig> GetConfig(CancellationToken cancellationToken = default);
    }
}
