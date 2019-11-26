using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pb.Wechat.MpSolicitudeTexts.Dto
{
    [Description("年龄段类型")]
    public enum SolicitudeTextType
    {
        [Description("怀孕")]
        UnBorn =0,
        [Description("0-1岁")]
        OneYear =1,
        [Description("1岁以上")]
        Over =2
    }
}
