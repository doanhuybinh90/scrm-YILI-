using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.TaskGroupMessages;

namespace Pb.Wechat.EntityFrameworkCore
{
    public class GroupMessageDbContext : AbpDbContext
    {
        public DbSet<TaskGroupMessage> TaskGroupMessages { get; set; }
        public GroupMessageDbContext(DbContextOptions<GroupMessageDbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
        }
    }
}
