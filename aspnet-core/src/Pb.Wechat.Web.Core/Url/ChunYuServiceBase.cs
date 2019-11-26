using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Configuration;

namespace Pb.Wechat.Web.Url
{
    public abstract class ChunYuServiceBase
    {

        public abstract string ChunYuPartnerFormatKey { get; }

        public abstract string ChunYuPasswordFormatKey { get; }

        public abstract string ChunYuBaseUrlFormatKey { get; }

        public abstract string ChunYuCreateUrlFormatKey { get; }

        public abstract string ChunYuCreateAddUrlFormatKey { get; }

        public abstract string ChunYuCloseUrlFormatKey { get; }

        public abstract string ChunYuLoginUrlFormatKey { get; }

        public string ChunYuPartner => _hostingEnvironment.GetAppConfiguration()[ChunYuPartnerFormatKey] ?? "yilinaifen";

        public string ChunYuPassword => _hostingEnvironment.GetAppConfiguration()[ChunYuPasswordFormatKey] ?? "pnivxaKfQw3IuhAz";

        public string ChunYuBaseUrl => _hostingEnvironment.GetAppConfiguration()[ChunYuBaseUrlFormatKey] ?? "http://test.chunyu.me";

        public string ChunYuCreateUrl => _hostingEnvironment.GetAppConfiguration()[ChunYuCreateUrlFormatKey] ?? "/cooperation/server/free_problem/create";

        public string ChunYuCreateAddUrl => _hostingEnvironment.GetAppConfiguration()[ChunYuCreateAddUrlFormatKey] ?? "/cooperation/server/problem_content/create";

        public string ChunYuCloseUrl => _hostingEnvironment.GetAppConfiguration()[ChunYuCloseUrlFormatKey] ?? "/cooperation/server/problem/close";

        public string ChunYuLoginUrl => _hostingEnvironment.GetAppConfiguration()[ChunYuLoginUrlFormatKey] ?? "/cooperation/server/login";

        
        private readonly IHostingEnvironment _hostingEnvironment;

        public ChunYuServiceBase(
            IHostingEnvironment hostingEnvironment
        )
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }
}
