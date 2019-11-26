using Abp.AutoMapper;
using Pb.Wechat.MpGroups.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpGroups
{
    [AutoMapFrom(typeof(MpGroupDto))]
    public class CreateOrEditMpGroupViewModel : MpGroupDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpGroupViewModel(MpGroupDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpGroupViewModel() {
        }
    }
}
