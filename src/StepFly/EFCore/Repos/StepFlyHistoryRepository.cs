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

        public Task<List<StepFlyHistory>> GetHistories(int pageIndex, int pageNum, StepFlyProviderType type)
        {
            return Task.FromResult(DbSet.Where(s => s.Source == (int)type)
                                        .OrderByDescending(s => s.CreationTime)
                                        .Skip((pageIndex - 1) * pageNum).Take(pageNum).ToList());
        }

        public Task<List<StepFlyHistory>> GetHistoriesByUser(int pageIndex, int pageNum, string userKey, StepFlyProviderType type)
        {
            return Task.FromResult(DbSet.Where(s => s.Source == (int)type && s.UserKeyInfo.Equals(userKey))
                                        .OrderByDescending(s => s.CreationTime)
                                        .Skip((pageIndex - 1) * pageNum).Take(pageNum).ToList());
        }

        public Task<StepFlyHistory> GetUserLastRecord(string userKey, StepFlyProviderType type)
        {
            return Task.FromResult(DbSet.OrderByDescending(s => s.CreationTime)
                                        .FirstOrDefault(s => s.Source == (int)type && s.UserKeyInfo.Equals(userKey)));
        }
    }
}
