using Abp.AutoMapper;
using Pb.Wechat.MpFans.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpFans
{
    [AutoMapFrom(typeof(MpFanDto))]
    public class CreateOrEditMpFanViewModel : MpFanDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpFanViewModel(MpFanDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpFanViewModel() {
        }
    }
}
