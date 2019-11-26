using Abp.Modules;
using Abp.Threading.BackgroundWorkers;
using System.Reflection;

namespace Pb.Hangfire
{
    public class PbHangfireModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
                //workManager.Add(IocManager.Resolve<WeChatSendMessageJob>());
                //workManager.Add(IocManager.Resolve<WechatGroupRefreshJob>());
            }
        }
    }
}
