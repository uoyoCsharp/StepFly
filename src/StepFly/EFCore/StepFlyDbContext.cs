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
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<StepFlyHistory> StepFlyHistories { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<VIPUser> VIPUsers { get; set; }

        public StepFlyDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //添加索引
            modelBuilder.Entity<StepFlyUser>(s =>
            {
                s.HasIndex(j => j.UserKeyInfo);
            });

            modelBuilder.Entity<FeedBack>(s =>
            {
                s.HasIndex(j => j.UserKey);
            });

            modelBuilder.Entity<StepFlyHistory>(s =>
            {
                s.HasIndex(j => j.UserKeyInfo);
            });

            modelBuilder.Entity<VIPUser>(s =>
            {
                s.HasIndex(j => j.UserId);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
