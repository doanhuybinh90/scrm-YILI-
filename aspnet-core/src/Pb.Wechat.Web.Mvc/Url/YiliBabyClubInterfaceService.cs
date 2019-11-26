using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Url
{
    public class YiliBabyClubInterfaceService : YiliBabyClubInterfaceServiceBase, IYiliBabyClubInterfaceService, ITransientDependency
    {
        public YiliBabyClubInterfaceService(
          IHostingEnvironment hostingEnvironment) :
          base(hostingEnvironment)
        {
        }

        public override string AesCacheKeyFormatKey => "App:YiliBabyClubInterface:AesCacheKey";

        public override string ApiUserNameFormatKey => "App:YiliBabyClubInterface:ApiUserName";

        public override string ApiPasswordFormatKey => "App:YiliBabyClubInterface:ApiPassword";

        public override string ApiDeviceCodeFormatKey => "App:YiliBabyClubInterface:ApiDeviceCode";

        public override string YiliwechatFormatKey => "App:YiliBabyClubInterface:Yiliwechat";

        public override string ApiTokenUrlFormatKey => "App:YiliBabyClubInterface:ApiTokenUrl";

        public override string MallApiTokenFormatKey => "App:YiliBabyClubInterface:MallApiToken";

        public override string GetMessageResultUrlFormatKey => "App:YiliBabyClubInterface:GetMessageResultUrl";
        public override string OAuthPathFormatKey => "App:YiliBabyClubInterface:OAuthPath";

    }
}
