using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpEvents.Dto
{
    [AutoMap(typeof(MpEvent))]
    public class MpEventDto : EntityDto<int>
    {
        
        public int MpID { get; set; }
       
        public string EventType { get; set; }

        [StringLength(200)]
        public string EventCode { get; set; }
       
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

        
    }
}
