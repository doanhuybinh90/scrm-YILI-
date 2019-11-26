using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Pb.Wechat.MpGroups.Dto
{
    [AutoMapFrom(typeof(MpGroup))]
    public class MpGroupTreeDto : AuditedEntityDto<int>
    {
      
        public int ParentId { get; set; }

        public string Name { get; set; }

    }
}
