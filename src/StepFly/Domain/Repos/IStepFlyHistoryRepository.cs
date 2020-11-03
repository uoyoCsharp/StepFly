using MiCake.DDD.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StepFly.Domain.Repos
{
    public interface IStepFlyHistoryRepository : IRepository<StepFlyHistory, long>
    {
        Task<List<StepFlyHistory>> GetHistories(int pageIndex, int pageNum, StepFlyProviderType type);

        Task<List<StepFlyHistory>> GetHistoriesByUser(int pageIndex, int pageNum, string userKey, StepFlyProviderType type);

        Task<StepFlyHistory> GetUserLastRecord(string userKey, StepFlyProviderType type);
    }
}
