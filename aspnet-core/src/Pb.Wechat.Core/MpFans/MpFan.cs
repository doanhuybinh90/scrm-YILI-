using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpFans
{
    public class MpFan : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "公众号ID不能为空")]
        public int MpID { get; set; }
        [StringLength(50)]
        public string OpenID { get; set; }
        [Description("会员ID")]
        public int MemberID { get; set; }
        public string UnionID { get; set; }
        [StringLength(100)]
        public string NickName { get; set; }
        public string Sex { get; set; }
        public string Language { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [StringLength(100)]
        public string Province { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        public string HeadImgUrl { get; set; }
        public DateTime? SubscribeTime { get; set; }
        public DateTime? FirstSubscribeTime { get; set; }

        public int? WxGroupID { get; set; }
        public int? GroupID { get; set; }
        public string GroupName { get; set; }
        public bool IsFans { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UnionName { get; set; }
        public int? ChannelID { get; set; }
        [StringLength(200)]
        public string ChannelName { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        [StringLength(50)]
        public string ChannelType { get; set; }
        public int LastCustomerServiceId { get; set; }
        [StringLength(100)]
        public string LastCustomerServiceOpenId { get; set; }
        public string TagNames { get; set; }
    }
}
