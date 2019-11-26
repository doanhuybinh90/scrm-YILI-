using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pb.Wechat.CustomerServiceOnlines.Dto
{
    [Description("客服类型")]
    public enum KFType
    {
        [Description("微信客服")]
        WX =0,
        [Description("伊利客服")]
        YL =1
    }
}
