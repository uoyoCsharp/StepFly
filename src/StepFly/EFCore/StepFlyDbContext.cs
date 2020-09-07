using MiCake.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StepFly.Domain;

namespace StepFly.EFCore
{
    public class StepFlyDbContext : MiCakeDbContext
    {
        public virtual DbSet<StepFlyUser> StepFlyUsers { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<HomeConfig> HomeConfig { get; set; }

        public StepFlyDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //添加索引
            modelBuilder.Entity<StepFlyUser>().HasIndex(s => s.UserKeyInfo);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
