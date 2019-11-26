using Abp.AutoMapper;
using Pb.Wechat.MpMediaVideos.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaVideos
{
    [AutoMapFrom(typeof(MpMediaVideoDto))]
    public class CreateOrEditMpMediaVideoViewModel : MpMediaVideoDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpMediaVideoViewModel(MpMediaVideoDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpMediaVideoViewModel() {
        }
    }
}
