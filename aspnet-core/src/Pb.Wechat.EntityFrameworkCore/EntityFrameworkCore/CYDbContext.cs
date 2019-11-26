using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.CYConfigs;
using Pb.Wechat.CYDoctors;
using Pb.Wechat.CYProblemContents;
using Pb.Wechat.CYProblems;

namespace Pb.Wechat.EntityFrameworkCore
{
    public class CYDbContext : AbpDbContext
    {
        public DbSet<CYDoctor> CYDoctors { get; set; }
        public DbSet<CYProblemContent> CYProblemContents { get; set; }
        public DbSet<CYProblem> CYProblems { get; set; }
        public DbSet<CYConfig> CYConfigs { get; set; }
        public CYDbContext(DbContextOptions<CYDbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CYProblemContent>(b =>
            {
                b.HasIndex(e => new { e.ProblemId });
            });

            modelBuilder.Entity<CYDoctor>(b =>
            {
                b.HasIndex(e => new { e.CYId });
            });
        }
    }
}
