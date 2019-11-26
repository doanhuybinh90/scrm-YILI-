using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pb.Wechat.Configuration;
using Pb.Wechat.Web;

namespace Pb.Wechat.EntityFrameworkCore
{
    public class KfDbContextFactory : IDesignTimeDbContextFactory<KfDbContext>
    {
        public KfDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<KfDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            KfDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AbpZeroTemplateConsts.ConnectionStringKf));

            return new KfDbContext(builder.Options);
        }
    }
}
