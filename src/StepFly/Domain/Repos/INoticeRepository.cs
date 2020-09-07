using MiCake.DDD.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Domain
{
    public interface INoticeRepository : IRepository<Notice, int>
    {
        Task<List<Notice>> GetValidNotices(CancellationToken cancellationToken = default);
    }
}
