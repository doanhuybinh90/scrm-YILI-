using Abp.AutoMapper;
using Pb.Wechat.MpSelfArticleGroups.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpSelfArticleGroups
{
    [AutoMapFrom(typeof(MpSelfArticleGroupDto))]
    public class CreateOrEditMpSelfArticleGroupViewModel : MpSelfArticleGroupDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpSelfArticleGroupViewModel(MpSelfArticleGroupDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpSelfArticleGroupViewModel() {
        }
    }
}
