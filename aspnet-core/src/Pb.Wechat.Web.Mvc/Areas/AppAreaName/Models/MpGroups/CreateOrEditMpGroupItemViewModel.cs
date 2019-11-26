using Abp.AutoMapper;
using Pb.Wechat.MpGroups.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpGroups
{
    [AutoMapFrom(typeof(MpGroupItemDto))]
    public class CreateOrEditMpGroupItemViewModel : MpGroupItemDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpGroupItemViewModel(MpGroupItemDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpGroupItemViewModel() {
        }
    }
}
