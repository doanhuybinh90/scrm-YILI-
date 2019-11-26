using Abp.Dependency;

namespace Pb.Wechat
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string TempFileDownloadFolder { get; set; }

        public string SampleProfileImagesFolder { get; set; }

        public string WebLogsFolder { get; set; }

        public string QrCodeDownloadFolder { get; set; }
    }
}