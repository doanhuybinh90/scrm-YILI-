using Abp.AutoMapper;
using Pb.Wechat.MpEvents.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpEvents
{
    [AutoMapFrom(typeof(MpEventDto))]
    public class CreateOrEditMpEventViewModel : MpEventDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpEventViewModel(MpEventDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpEventViewModel() {
        }
    }
}
