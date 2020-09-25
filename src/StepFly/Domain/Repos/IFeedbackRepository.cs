using MiCake.DDD.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Domain.Repos
{
    public interface IFeedbackRepository : IRepository<FeedBack, int>
    {
        Task<List<FeedBack>> GetFeedBacks(int pageIndex, int pageNum, CancellationToken cancellationToken = default);

        Task<List<FeedBack>> GetUserTodayFeedbacks(string userKey, CancellationToken cancellationToken = default);
    }
}
