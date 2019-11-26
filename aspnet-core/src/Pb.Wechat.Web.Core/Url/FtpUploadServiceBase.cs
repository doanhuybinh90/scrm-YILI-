using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Configuration;

namespace Pb.Wechat.Web.Url
{
    public abstract class FtpUploadServiceBase
    {
        public abstract string ServerDomainFormatKey { get; }

        public abstract string UserNameFormatKey { get; }

        public abstract string PasswordFormatKey { get; }

        public abstract string ViewUrlFormatKey { get; }
        public string ServerDomain => _hostingEnvironment.GetAppConfiguration()[ServerDomainFormatKey] ?? "www.mgcc.com.cn";

        public string UserName => _hostingEnvironment.GetAppConfiguration()[UserNameFormatKey] ?? "123";

        public string Password => _hostingEnvironment.GetAppConfiguration()[PasswordFormatKey] ?? "123";

        public string ViewUrl => _hostingEnvironment.GetAppConfiguration()[ViewUrlFormatKey] ?? "http://www.mgcc.com.cn/";

        private readonly IHostingEnvironment _hostingEnvironment;

        public FtpUploadServiceBase(
            IHostingEnvironment hostingEnvironment
        )
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }
}
