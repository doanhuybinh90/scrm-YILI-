using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.MpMediaVoices.Dto
{
    [AutoMap(typeof(MpMediaVoice))]
    public class MpMediaVoiceDto : EntityDto<int>
    {

        /// <summary>
        /// MpID
        /// </summary>
        [Description("MpID")]
        public int MpID { get; set; }

        /// <summary>
        /// MediaID
        /// </summary>
        [Description("MediaID")]
        public string MediaID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [Description("Title")]
        public string Title { get; set; }
        [Description("Description")]
        public string Description { get; set; }

        /// <summary>
        /// FileID
        /// </summary>
        [Description("FileID")]
        public string FileID { get; set; }

        public string FilePathOrUrl { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
