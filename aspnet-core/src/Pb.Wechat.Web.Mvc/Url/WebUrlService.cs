using Abp.Dependency;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Url;

namespace Pb.Wechat.Web.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IHostingEnvironment hostingEnvironment,
            ITenantCache tenantCache) :
            base(hostingEnvironment, tenantCache)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string WebSiteRootIPFormatKey => "App:WebSiteRootIP";
        public override string RemotingFileUploadUrl => "App:RemotingFileUploadUrl";

        public override string RemotingFileDownloadUrl => "App:RemotingFileDownloadUrl";
        public override string KindEditorSavePath => "App:KindEditorSavePath";

    }
}