using Abp.AutoMapper;
using Pb.Wechat.MpBabyTexts.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpBabyTexts
{
    [AutoMapFrom(typeof(MpBabyTextDto))]
    public class CreateOrEditMpBabyTextViewModel : MpBabyTextDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditMpBabyTextViewModel(MpBabyTextDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpBabyTextViewModel() {
        }
    }
}
