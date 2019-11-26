using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pb.Wechat.Configuration;
using Pb.Wechat.Web;

namespace Pb.Wechat.EntityFrameworkCore
{
    public class CYDbContextFactory : IDesignTimeDbContextFactory<CYDbContext>
    {
        public CYDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CYDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            CYDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AbpZeroTemplateConsts.ConnectionStringCY));

            return new CYDbContext(builder.Options);
        }
    }
}
