using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.Url
{
    public interface ISignalrCustomerService
    {
        string CustomerAskUrl { get; }
        string SetUnConnectNoticeUrl { get; }
    }
}
