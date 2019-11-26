using System.ComponentModel;

namespace Pb.Wechat.MpGroups.Dto
{
    public enum MotherType
    {
        [Description("全部")]
        ALL,
        [Description("待孕")]
        UnPregnant,
        [Description("怀孕")]
        Pregnant,
        [Description("0-6个月")]
        One,
        [Description("6-12个月")]
        Two,
        [Description("1-2岁")]
        Three,
        [Description("2岁以上")]
        Four,
        [Description("其他")]
        Other

    }
}
