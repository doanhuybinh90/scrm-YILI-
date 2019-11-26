using Abp.AutoMapper;
using Pb.Wechat.MpMediaArticles.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaArticles
{
    [AutoMapFrom(typeof(MpMediaArticleDto))]
    public class CreateOrEditMpMediaArticleViewModel : MpMediaArticleDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpMediaArticleViewModel(MpMediaArticleDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpMediaArticleViewModel() {
        }
    }
}
