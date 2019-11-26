using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Url;

namespace Pb.Wechat.Web.Url
{
    public class ChunYuService : ChunYuServiceBase, IChunYuService, ITransientDependency
    {
        public ChunYuService(
            IHostingEnvironment hostingEnvironment) :
            base(hostingEnvironment)
        {
        }

        public override string ChunYuPartnerFormatKey => "App:ChunYu:Partner";

        public override string ChunYuPasswordFormatKey => "App:ChunYu:Password";

        public override string ChunYuBaseUrlFormatKey => "App:ChunYu:BaseUrl";

        public override string ChunYuCreateUrlFormatKey => "App:ChunYu:CreateUrl";

        public override string ChunYuCreateAddUrlFormatKey => "App:ChunYu:CreateAddUrl";

        public override string ChunYuCloseUrlFormatKey => "App:ChunYu:CloseUrl";

        public override string ChunYuLoginUrlFormatKey => "App:ChunYu:LoginUrl";
    }
}
