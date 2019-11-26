using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pb.Wechat.MpMediaArticles.Dto
{
    [Description("留言类型")]
    public enum CommentTypeEnum
    {
        [Description("所有人")]
        All,
        [Description("粉丝")]
        Fans
    }
}
