using Abp.AutoMapper;
using Pb.Wechat.MpProductInfos.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpProductInfos
{
    [AutoMapFrom(typeof(MpProductInfoDto))]
    public class CreateOrEditMpProductInfoViewModel : MpProductInfoDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditMpProductInfoViewModel(MpProductInfoDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpProductInfoViewModel() {
        }
    }
}
