using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpGroupItems
{
    public class MpGroupItem : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "公众号ID不能为空")]
        public int MpID { get; set; }
        [Description("是否会员")]
        public string IsMember { get; set; }

        [Description("父节点ID")]
        public int ParentID { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(500)]
        [Description("分组名称")]
        public string Name { get; set; }
        [Description("宝宝生日")]
        public int BaySex { get; set; }
        [Description("管理区域")]
        public string OrganizeCity { get; set; }
        [Description("行政城市")]
        public string OfficialCity { get; set; }
        [Description("最后充值产品编码")]
        public string LastBuyProduct { get; set; }
        [Description("会员分类")]
        public string MemberCategory { get; set; }
        [Description("宝宝生日起始日")]
        public DateTime BeginBabyBirthday { get; set; }
        [Description("宝宝生日结束日")]
        public DateTime EndBabyBirthday { get; set; }
        [Description("剩余积分起始")]
        public int BeginPointsBalance { get; set; }
        [Description("剩余积分结束")]
        public int EndPointsBalance { get; set; }
        [Description("渠道ID")]
        public string ChannelID { get; set; }
        [Description("渠道名称")]
        public string ChannelName { get; set; }
        [Description("标签ID")]
        public string TargetID { get; set; }
        [Description("标签名称")]
        public string TargetName { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }

        public string MotherType { get; set; }
    }
}
