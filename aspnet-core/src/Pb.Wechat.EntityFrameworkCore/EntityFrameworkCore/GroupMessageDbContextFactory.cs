using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pb.Wechat.Configuration;
using Pb.Wechat.Web;

namespace Pb.Wechat.EntityFrameworkCore
{
    public class GroupMessageDbContextFactory : IDesignTimeDbContextFactory<GroupMessageDbContext>
    {
        public GroupMessageDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<GroupMessageDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            GroupMessageDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AbpZeroTemplateConsts.ConnectionStringGroupMessage));

            return new GroupMessageDbContext(builder.Options);
        }
    }
}
