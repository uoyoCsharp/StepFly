using MiCake.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using StepFly.Domain;
using StepFly.Domain.Repos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.EFCore.Repos
{
    public class VIPUserRepository : EFRepository<StepFlyDbContext, VIPUser, int>, IVIPUserRepository
    {
        public VIPUserRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<VIPUser> FindByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);
        }
    }
}
