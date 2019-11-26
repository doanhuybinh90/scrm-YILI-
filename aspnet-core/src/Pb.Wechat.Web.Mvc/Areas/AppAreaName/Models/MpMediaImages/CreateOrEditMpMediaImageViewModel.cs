using Abp.AutoMapper;
using Pb.Wechat.MpMediaImages.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaImages
{
    [AutoMapFrom(typeof(MpMediaImageDto))]
    public class CreateOrEditMpMediaImageViewModel : MpMediaImageDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpMediaImageViewModel(MpMediaImageDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpMediaImageViewModel() {
        }
    }
}
