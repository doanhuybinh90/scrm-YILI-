using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.CustomerServiceResponseTexts.Dto
{
    [AutoMap(typeof(CustomerServiceResponseText))]
    public class CustomerServiceResponseTextDto : EntityDto<int>
    {
        public int MpID { get; set; }
        public string ResponseType { get; set; }
        public string ResponseText { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int UseCount { get; set; }
        public int? ReponseContentType { get; set; }
        public string PreviewImgUrl { get; set; }
        public string MediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public string ImageName { get; set; }
        public string VoiceName { get; set; }
    }
}
