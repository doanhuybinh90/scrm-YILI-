using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.TaskGroupMessages
{
    public class TaskGroupMessage : Entity<long>, IAudited, ISoftDelete
    {
        public int TaskState { get; set; }
        public string TaskID { get; set; }
        public int GroupID { get; set; }
        public int MessageID { get; set; }
        public int MpID { get; set; }
        public string OpenIDs { get; set; }
        public int SendCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public string WxMsgID { get; set; }
        public string ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public string MsgDataID { get; set; }
        public string ClearedOpenIDs { get; set; }
        //public string MessageType { get; set; }


        //public string Content { get; set; }
        //public int? ArticleID { get; set; }
        //[StringLength(500)]
        //public string ArticleName { get; set; }
        //[StringLength(200)]
        //public string ArticleMediaID { get; set; }
        //public int? ArticleGroupID { get; set; }
        //[StringLength(500)]
        //public string ArticleGroupName { get; set; }
        //[StringLength(200)]
        //public string ArticleGroupMediaID { get; set; }
        //public int? ImageID { get; set; }
        //[StringLength(500)]
        //public string ImageName { get; set; }
        //[StringLength(200)]
        //public string ImageMediaID { get; set; }
        //public int? VideoID { get; set; }
        //[StringLength(500)]
        //public string VideoName { get; set; }
        //[StringLength(200)]
        //public string VideoMediaID { get; set; }
        //public int? VoiceID { get; set; }
        //[StringLength(500)]
        //public string VoiceName { get; set; }
        //[StringLength(200)]
        //public string VoiceMediaID { get; set; }


        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }

    }
}
