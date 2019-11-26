using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpChannels
{
    public class MpChannel : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "公众号ID不能为空")]
        public int MpID { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(500)]
        [Description("二维码名称")]
        public string Name { get; set; }
        [StringLength(500)]
        public string PushActivityName { get; set; }
        [Description("参数")]
        public string EventKey { get; set; }
        [Description("渠道编号")]
        public string Code { get; set; }
        [StringLength(500)]
        [Description("渠道名称")]
        public string ChannelName { get; set; }
        [Description("微信带参二维码票据")]
        public string Ticket { get; set; }
        [Description("微信带参二维码图片路径")]
        public string FilePath { get; set; }
        [Description("微信带参二维码图片url")]
        public string FileUrl { get; set; }
        [Description("带参二维码类型")]
        public string ChannelType { get; set; }

        [Required(ErrorMessage = "回复类型不能为空")]
        [StringLength(50)]
        public string ReplyType { get; set; }
        public string Content { get; set; }
        public int IsMcChannel { get; set; }
        public int? ArticleID { get; set; }
        [StringLength(500)]
        public string ArticleName { get; set; }
        [StringLength(200)]
        public string ArticleMediaID { get; set; }

        public int? ArticleGroupID { get; set; }
        [StringLength(500)]
        public string ArticleGroupName { get; set; }
        [StringLength(200)]
        public string ArticleGroupMediaID { get; set; }

        public int? ImageID { get; set; }
        [StringLength(500)]
        public string ImageName { get; set; }
        [StringLength(200)]
        public string ImageMediaID { get; set; }

        public int? VideoID { get; set; }
        [StringLength(500)]
        public string VideoName { get; set; }
        [StringLength(200)]
        public string VideoMediaID { get; set; }

        public int? VoiceID { get; set; }
        [StringLength(500)]
        public string VoiceName { get; set; }
        [StringLength(200)]
        public string VoiceMediaID { get; set; }
        [Description("生效天数")]
        public int ValidityDay { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }

        [StringLength(500)]
        public string TagIds { get; set; }
        [StringLength(2000)]
        public string TagNames { get; set; }
    }
}
