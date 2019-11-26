using Abp.AutoMapper;
using Pb.Wechat.MpMenus.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpMenus
{
    [AutoMapFrom(typeof(MpMenuDto))]
    public class CreateOrEditMpMenuViewModel : MpMenuDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpMenuViewModel(MpMenuDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpMenuViewModel() {
        }
    }
}
