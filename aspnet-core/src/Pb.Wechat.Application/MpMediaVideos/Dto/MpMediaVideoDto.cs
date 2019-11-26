using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.MpMediaVideos.Dto
{
    [AutoMap(typeof(MpMediaVideo))]
    public class MpMediaVideoDto : EntityDto<int>
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
        /// Title
        /// </summary>
        [Description("Title")]
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>
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
