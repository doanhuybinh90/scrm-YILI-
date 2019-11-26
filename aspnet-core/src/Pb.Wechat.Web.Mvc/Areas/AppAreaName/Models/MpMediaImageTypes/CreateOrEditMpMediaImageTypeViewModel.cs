using Abp.AutoMapper;
using Pb.Wechat.MpMediaImageTypes.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaImageTypes
{
    [AutoMapFrom(typeof(MpMediaImageTypeDto))]
    public class CreateOrEditMpMediaImageTypeViewModel : MpMediaImageTypeDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpMediaImageTypeViewModel(MpMediaImageTypeDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpMediaImageTypeViewModel() {
        }
    }
}
