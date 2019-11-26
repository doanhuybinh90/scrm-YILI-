using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpUserMembers
{
    public class MpUserMember : Entity<int>, IAudited, ISoftDelete
    {
        [Description("OpenID")]
        [StringLength(50)]
        public string OpenID { get; set; }
        
        public string UnionID { get; set; }
        
        [Description("宝宝生日")]
        public DateTime? BabyBirthday { get; set; }
        
        [Description("手机号")]
        [StringLength(50)]
        public string MobilePhone { get; set; }

        [Description("邮箱")]
        [StringLength(150)]
        public string Email { get; set; }

        [Description("顾客姓名")]
        [StringLength(100)]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "用户名不能为空")]
        [Description("用户名")]
        [StringLength(50)]
        public string MemeberUserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [Description("顾客密码")]
        [StringLength(50)]
        public string MemberPassword { get; set; }
        
        [Description("宝宝姓名")]
        [StringLength(100)]
        public string BabyName { get; set; }
        [Description("省")]
        public string Province { get; set; }
        [Description("市")]
        public string City { get; set; }
        [Description("区")]
        public string Area { get; set; }
        [Description("街道")]
        public string Street { get; set; }
        [Description("宝宝关系")]
        public string MemberIdentity { get; set; }
        [Description("用过品牌")]
        public int? UsedBrand { get; set; }
        [Description("注册产品")]
        public string RegistProduct { get; set; }
        [Description("喂养方式")]
        public int? FeedingMode { get; set; }
        [Description("每月用量")]
        public decimal? MonthConsumption { get; set; }
        [Description("每天用量")]
        public int? DayConsumption { get; set; }
        [Description("服务渠道")]
        public int? ServiceChannel { get; set; }
        [Description("服务门店")]
        public string ServiceShop { get; set; }
        [Description("服务门店编号")]
        public string ServiceShopCode { get; set; }
        [Description("众引接口AuthCache")]
        [StringLength(50)]
        public string MgccAuthkey { get; set; }
        [StringLength(50)]
        public string MemberState { get; set; } 
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        [Description("关注时间")]
        public DateTime? SubscribeTime { get; set; }
        [Description("注册时间")]
        public DateTime? RegisterTime { get; set; }
        [Description("绑定时间")]
        public DateTime? BindingTime { get; set; }
        [Description("偏好渠道")]
        public string LikeChannel { get; set; }
        [Description("爱好")]
        public string Hobby { get; set; }
        [Description("家庭角色")]
        public int? HomeRole { get; set; }
        [Description("行政城市")]
        public int? OfficialCity { get; set; }
        [Description("家庭地址")]
        public string Address { get; set; }
        [Description("激活时间")]
        public DateTime? ActiveDate { get; set; }
        [Description("CRMID")]
        public int? CRMID { get; set; }
        [Description("CEM用户ID")]
        public Guid? CRMUserID { get; set; }
        [Description("会员级别")]
        public int? MemberType { get; set; }
        public bool? EmailVerifyFlag { get; set; }
        public int? Sex { get; set; }
        public string Atts { get; set; }
        public string RegistProductName { get; set; }
        public int ChannelID { get; set; }
        [StringLength(500)]
        public string ChannelName { get; set; }
        public bool IsBinding { get; set; }
        [StringLength(500)]
        public string UnBindingReason { get; set; }
    }
}
