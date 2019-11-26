using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.Url
{
    public interface IYiliBabyClubInterfaceService
    {

        string AesCacheKey { get; }

        string ApiUserName { get; }

        string ApiPassword { get; }

        string ApiDeviceCode { get; }

        string Yiliwechat { get; }

        string ApiTokenUrl { get; }

        string MallApiToken { get; }
        string GetMessageResultUrl { get; }

        string OAuthPath { get; }
    }
}
