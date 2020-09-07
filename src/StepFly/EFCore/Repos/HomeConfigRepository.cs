using MiCake.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using StepFly.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.EFCore.Repos
{
    public class HomeConfigRepository : EFRepository<StepFlyDbContext, HomeConfig, int>, IHomeConfigRepository
    {
        public HomeConfigRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<HomeConfig> GetConfig(CancellationToken cancellationToken = default)
        {
            return DbSet.FirstOrDefaultAsync();
        }
    }
}
