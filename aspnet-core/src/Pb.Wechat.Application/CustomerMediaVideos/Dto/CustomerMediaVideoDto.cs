using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.CustomerMediaVideos.Dto
{
    [AutoMap(typeof(CustomerMediaVideo))]
    public class CustomerMediaVideoDto : EntityDto<int>
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

        public string FilePathOrUrl { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
