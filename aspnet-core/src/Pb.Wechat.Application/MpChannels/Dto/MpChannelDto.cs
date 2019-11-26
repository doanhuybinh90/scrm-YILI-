using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.MpChannels.Dto
{
    [AutoMap(typeof(MpChannel))]
    public class MpChannelDto : EntityDto<int>
    {

        public int MpID { get; set; }

        public string Name { get; set; }

        public string PushActivityName { get; set; }

        public string EventKey { get; set; }
 
        public string Code { get; set; }

        public string ChannelName { get; set; }

        public string Ticket { get; set; }

        public string FilePath { get; set; }

        public string FileUrl { get; set; }

        public string ChannelType { get; set; }


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

        public int ValidityDay { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int IsMcChannel { get; set; }
        
        public string TagIds { get; set; }
        public string TagNames { get; set; }
    }
}
