using Abp.AutoMapper;
using Pb.Wechat.MpMediaArticleGroups.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaArticleGroups
{
    [AutoMapFrom(typeof(MpMediaArticleGroupDto))]
    public class CreateOrEditMpMediaArticleGroupViewModel : MpMediaArticleGroupDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpMediaArticleGroupViewModel(MpMediaArticleGroupDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpMediaArticleGroupViewModel() {
        }
    }
}
