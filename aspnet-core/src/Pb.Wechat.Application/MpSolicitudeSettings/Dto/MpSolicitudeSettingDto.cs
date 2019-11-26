using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel;

namespace Pb.Wechat.MpSolicitudeSettings.Dto
{
    [AutoMap(typeof(MpSolicitudeSetting))]
    public class MpSolicitudeSettingDto : EntityDto
    {
        /// <summary>
        /// MpID
        /// </summary>
        [Description("MpID")]
        public int MpID { get; set; }
        public int DelayMinutes { get; set; }
        /// <summary>
        /// FileID
        /// </summary>
        [Description("MediaType")]
        public string MediaType { get; set; }
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
    }
}
