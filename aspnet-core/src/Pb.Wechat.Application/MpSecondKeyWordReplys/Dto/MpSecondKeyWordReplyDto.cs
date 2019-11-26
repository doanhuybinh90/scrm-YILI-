using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace Pb.Wechat.MpSecondKeyWordReplys.Dto
{
    [AutoMap(typeof(MpSecondKeyWordReply))]
    public class MpSecondKeyWordReplyDto : EntityDto
    {
        public int MpID { get; set; }
        public int ParentId { get; set; }
        public string KeyWord { get; set; }
        public string ReplyType { get; set; }
        public string Content { get; set; }

        public int? ArticleID { get; set; }
        public string ArticleName { get; set; }
        public string ArticleMediaID { get; set; }

        public int? ArticleGroupID { get; set; }
        public string ArticleGroupName { get; set; }
        public string ArticleGroupMediaID { get; set; }

        public int? ImageID { get; set; }
        public string ImageName { get; set; }
        public string ImageMediaID { get; set; }

        public int? VideoID { get; set; }
        public string VideoName { get; set; }
        public string VideoMediaID { get; set; }

        public int? VoiceID { get; set; }
        public string VoiceName { get; set; }
        public string VoiceMediaID { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
