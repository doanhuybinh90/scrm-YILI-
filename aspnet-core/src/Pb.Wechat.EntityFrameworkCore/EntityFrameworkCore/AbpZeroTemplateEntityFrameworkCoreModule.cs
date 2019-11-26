using Abp.EntityFrameworkCore.Configuration;
using Abp.IdentityServer4;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Pb.Wechat.Configuration;
using Pb.Wechat.Migrations.Seed;

namespace Pb.Wechat.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpZeroCoreEntityFrameworkCoreModule),
        typeof(AbpZeroTemplateCoreModule),
        typeof(AbpZeroCoreIdentityServerEntityFrameworkCoreModule)
        )]
    public class AbpZeroTemplateEntityFrameworkCoreModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        public AbpZeroTemplateEntityFrameworkCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<AbpZeroTemplateDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        AbpZeroTemplateDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        AbpZeroTemplateDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
                Configuration.Modules.AbpEfCore().AddDbContext<CYDbContext>(options =>
                {
                    
                    if (options.ExistingConnection != null)
                    {
                        CYDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        CYDbContextConfigurer.Configure(options.DbContextOptions, _appConfiguration.GetConnectionString(
                            AbpZeroTemplateConsts.ConnectionStringCY
                        ));
                    }
                });
                Configuration.Modules.AbpEfCore().AddDbContext<GroupMessageDbContext>(options =>
                {

                    if (options.ExistingConnection != null)
                    {
                        GroupMessageDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        GroupMessageDbContextConfigurer.Configure(options.DbContextOptions, _appConfiguration.GetConnectionString(
                            AbpZeroTemplateConsts.ConnectionStringGroupMessage
                        ));
                    }
                });
                Configuration.Modules.AbpEfCore().AddDbContext<KfDbContext>(options =>
                {

                    if (options.ExistingConnection != null)
                    {
                        KfDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        KfDbContextConfigurer.Configure(options.DbContextOptions, _appConfiguration.GetConnectionString(
                            AbpZeroTemplateConsts.ConnectionStringKf
                        ));
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroTemplateEntityFrameworkCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
