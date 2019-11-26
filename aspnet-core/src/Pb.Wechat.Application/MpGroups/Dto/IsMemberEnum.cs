using System.ComponentModel;

namespace Pb.Wechat.MpGroups.Dto
{
    public enum IsMemberEnum
    {
        [Description("所有粉丝")]
        ALL,
        [Description("按标签筛选")]
        Tag,
        [Description("会员")]
        Member,
        [Description("非会员")]
        NotMember
    }
}
