using Abp.AutoMapper;
using Pb.Wechat.MpGroups.Dto;
using Pb.Wechat.MpMessages.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpMessages
{
    [AutoMapFrom(typeof(MpMessageDto))]
    public class CreateOrEditMpMessageViewModel : MpMessageDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpMessageViewModel(MpMessageDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpMessageViewModel() {
        }
        public IEnumerable<MpGroupDto> GroupModels { get; set; }
    }
}
