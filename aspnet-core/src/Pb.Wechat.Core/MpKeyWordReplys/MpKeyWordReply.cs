using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpKeyWordReplys
{
    public class MpKeyWordReply : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "企业号ID不能为空")]
        public int MpID { get; set; }
        [Required(ErrorMessage = "关键字不能为空")]
        [StringLength(50)]
        public string KeyWord { get; set; }
 

        [Required(ErrorMessage = "回复类型不能为空")]
        [StringLength(50)]
        public string ReplyType { get; set; }
        public string Content { get; set; }

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

        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
