using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pb.Wechat.CustomerServiceOnlines.Dto
{
    [Description("连接状态")]
    public enum ConnectState
    {
        [Description("在线")]
        Connect = 1,
        [Description("掉线")]
        UnConnect = 2
    }

    [Description("在线状态")]
    public enum OnlineState
    {
        [Description("在线")]
        OnLine = 0,
        [Description("离开")]
        Leave =1,
        [Description("退出登录")]
        Quit = 2
    }
}
