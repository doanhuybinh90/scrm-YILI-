using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Pb.Wechat.Configuration;
using Pb.Wechat.MultiTenancy;
using Pb.Wechat.Web.Areas.AppAreaName.Startup;
using Pb.Wechat.Web.Resources.FileServers;
using Pb.Wechat.Web.Resources.WxMediaHelper;

namespace Pb.Wechat.Web.Startup
{
    [DependsOn(
        typeof(AbpZeroTemplateWebCoreModule)
    )]
    public class AbpZeroTemplateWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AbpZeroTemplateWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:WebSiteRootAddress"] ?? "http://localhost:62114/";

            Configuration.Navigation.Providers.Add<AppAreaNameNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroTemplateWebMvcModule).GetAssembly());
            //IocManager.Register<IFileServer, FtpFileServer>(DependencyLifeStyle.Transient);
            //IocManager.Register<IFileServer, LocalFileServer>(DependencyLifeStyle.Transient);
            IocManager.Register<IFileServer, MapDirFileServer>(DependencyLifeStyle.Transient);
            IocManager.Register<IWxMediaUpload, WxMediaUploadBaseHandler>(DependencyLifeStyle.Transient);

        }

        public override void PostInitialize()
        {
            if (!IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                return;
            }
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
            workManager.Add(IocManager.Resolve<SubscriptionExpireEmailNotifierWorker>());

        }
    }
}