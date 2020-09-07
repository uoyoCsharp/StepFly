using MiCake.EntityFrameworkCore.Repository;
using StepFly.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.EFCore.Repos
{
    public class NoticeRepository : EFRepository<StepFlyDbContext, Notice, int>, INoticeRepository
    {
        public NoticeRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<List<Notice>> GetValidNotices(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(DbSet.Where(s => s.IsEffective).ToList());
        }
    }
}
