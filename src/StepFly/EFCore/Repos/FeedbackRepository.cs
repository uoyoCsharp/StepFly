using MiCake.Core.Util;
using MiCake.EntityFrameworkCore.Repository;
using StepFly.Domain;
using StepFly.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.EFCore.Repos
{
    public class FeedbackRepository : EFRepository<StepFlyDbContext, FeedBack, int>, IFeedbackRepository
    {
        public FeedbackRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<List<FeedBack>> GetFeedBacks(int pageIndex, int pageNum, CancellationToken cancellationToken = default)
        {
            if (pageIndex < 1)
                throw new ArgumentException("page页数不正确");
            return Task.FromResult(DbSet.OrderByDescending(s => s.CreationTime).Skip((pageIndex - 1) * pageNum).Take(pageNum).ToList());
        }

        public Task<List<FeedBack>> GetUserTodayFeedbacks(string userKey, CancellationToken cancellationToken = default)
        {
            CheckValue.NotNullOrWhiteSpace(userKey, nameof(userKey));
            var nowDate = DateTime.Now;
            var currentDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day);
            return Task.FromResult(DbSet.Where(s => s.UserKey.Equals(userKey) && (s.CreationTime <= currentDate.AddDays(1) && s.CreationTime >= currentDate)).ToList());
        }
    }
}
