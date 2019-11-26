using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpAccounts
{
    public class MpAccount : Entity<int>, IAudited, ISoftDelete
    {
        [Description("微信号")]
        public string WxAccount { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "类型不能为空")]
        [StringLength(50)]
        public string AccountType { get; set; }
        [Required(ErrorMessage = "AppId不能为空")]
        [StringLength(200)]
        public string AppId { get; set; }
        [Required(ErrorMessage = "AppSecret不能为空")]
        [StringLength(500)]
        public string AppSecret { get; set; }
        [StringLength(200)]
        public string Token { get; set; }
        [StringLength(500)]
        public string EncodingAESKey { get; set; }
        [StringLength(200)]
        public string MchID { get; set; }
        [StringLength(500)]
        public string WxPayAppSecret { get; set; }
        [StringLength(500)]
        public string CertPhysicalPath { get; set; }
        [StringLength(50)]
        public string CertPassword { get; set; }
        public string Remark { get; set; }
        [Description("AccessToken过期时间-定时任务")]
        public DateTime? ExpireTime { get; set; }
        [Description("AccessToken-定时任务")]
        public string AccessToken { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        [Description("固定Token值")]
        public string TaskAccessToken { get; set; }
    }
}
