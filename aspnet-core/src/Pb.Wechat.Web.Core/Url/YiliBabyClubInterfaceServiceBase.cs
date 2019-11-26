using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.Web.Url
{
    public abstract class YiliBabyClubInterfaceServiceBase
    {

        public abstract string AesCacheKeyFormatKey { get; }

        public abstract string ApiUserNameFormatKey { get; }

        public abstract string ApiPasswordFormatKey { get; }

        public abstract string ApiDeviceCodeFormatKey { get; }

        public abstract string YiliwechatFormatKey { get; }

        public abstract string ApiTokenUrlFormatKey { get; }
        public abstract string GetMessageResultUrlFormatKey { get; }

        public abstract string MallApiTokenFormatKey { get; }

        public abstract string OAuthPathFormatKey { get; }


        public string AesCacheKey => _hostingEnvironment.GetAppConfiguration()[AesCacheKeyFormatKey] ?? "AesEncryptor";

        public string ApiUserName => _hostingEnvironment.GetAppConfiguration()[ApiUserNameFormatKey] ?? "anonymity";

        public string ApiPassword => _hostingEnvironment.GetAppConfiguration()[ApiPasswordFormatKey] ?? "anonymity";

        public string ApiDeviceCode => _hostingEnvironment.GetAppConfiguration()[ApiDeviceCodeFormatKey] ?? "WEDO_YILYI_WECHAT_MYLOCAL";

        public string Yiliwechat => _hostingEnvironment.GetAppConfiguration()[YiliwechatFormatKey] ?? "MGCC";

        public string ApiTokenUrl => _hostingEnvironment.GetAppConfiguration()[ApiTokenUrlFormatKey] ?? "MpApi/GetAccessToken";
        public string GetMessageResultUrl => _hostingEnvironment.GetAppConfiguration()[GetMessageResultUrlFormatKey] ?? "MpApi/GetSendMembersFromMC";
        public string MallApiToken => _hostingEnvironment.GetAppConfiguration()[MallApiTokenFormatKey] ?? "e1a9c051";
        public string OAuthPath => _hostingEnvironment.GetAppConfiguration()[OAuthPathFormatKey] ?? "http://t.wechat.yilibabyclub.com/H5.aspx";

        private readonly IHostingEnvironment _hostingEnvironment;
        public YiliBabyClubInterfaceServiceBase(
           IHostingEnvironment hostingEnvironment
       )
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }
}
