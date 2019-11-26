using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpMessages
{
    public class MpMessage : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "企业号ID不能为空")]
        public int MpID { get; set; }
        [Required(ErrorMessage = "类型不能为空")]
        [StringLength(50)]
        public string MessageType { get; set; }
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
        [StringLength(64)]
        public string WxMsgID { get; set; }
        public string State { get; set; }
        public long? SendCount { get; set; }
        public long? SuccessCount { get; set; }
        public long? FailCount { get; set; }
        public DateTime? FinishDate { get; set; }
        
        public string FirstLevelGroup { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public int IsTask { get; set; }
        public DateTime? ExecTaskTime { get; set; }
        public int SendState { get; set; }

        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }

        public string GroupName { get; set; }
        public int? GroupID { get; set; }
        public string IsMember { get; set; }
        public int BaySex { get; set; }
      
        public string OrganizeCity { get; set; }
      
        public string OfficialCity { get; set; }
       
        public string LastBuyProduct { get; set; }
      
        public string MemberCategory { get; set; }
        
        public DateTime BeginBabyBirthday { get; set; }
       
        public DateTime EndBabyBirthday { get; set; }
     
        public int BeginPointsBalance { get; set; }
       
        public int EndPointsBalance { get; set; }
      
        public string ChannelID { get; set; }
    
        public string ChannelName { get; set; }
   
        public string TargetID { get; set; }

        public string TargetName { get; set; }
        public string MotherType { get; set; }
        public string GroupIds { get; set; }
        public string SendMsgId { get; set; }
     
    }
}
