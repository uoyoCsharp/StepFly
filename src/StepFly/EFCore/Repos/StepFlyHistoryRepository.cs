using MiCake.EntityFrameworkCore.Repository;
using StepFly.Domain;
using StepFly.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StepFly.EFCore.Repos
{
    public class StepFlyHistoryRepository : EFRepository<StepFlyDbContext, StepFlyHistory, long>, IStepFlyHistoryRepository
    {
        public StepFlyHistoryRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<List<StepFlyHistory>> GetHistories(int pageIndex, int pageNum)
        {
            if (pageIndex < 1)
                throw new ArgumentException("page页数不正确");
            return Task.FromResult(DbSet.OrderByDescending(s => s.CreationTime).Skip((pageIndex - 1) * pageNum).Take(pageNum).ToList());
        }
    }
}
