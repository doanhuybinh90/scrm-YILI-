using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Url;

namespace Pb.Wechat.Web.Url
{
    public class FtpUploadService : FtpUploadServiceBase, IFtpUploadService, ITransientDependency
    {
        public FtpUploadService(
            IHostingEnvironment hostingEnvironment) :
            base(hostingEnvironment)
        {
        }

        public override string ServerDomainFormatKey => "App:FtpUpload:ServerDomain";

        public override string UserNameFormatKey => "App:FtpUpload:UserName";

        public override string PasswordFormatKey => "App:FtpUpload:Password";

        public override string ViewUrlFormatKey => "App:FtpUpload:ViewUrl";
    }
}
