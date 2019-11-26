using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.CustomerMediaImages.Dto
{
    [AutoMap(typeof(CustomerMediaImage))]
    public class CustomerMediaImageDto : EntityDto<int>
    {

        [Description("MpID")]
        public int MpID { get; set; }

        [Description("MediaID")]
        public string MediaID { get; set; }

        [Description("Name")]
        public string Name { get; set; }


        public string FilePathOrUrl { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int MediaImageType { get; set; }
        public string MediaImageTypeName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
