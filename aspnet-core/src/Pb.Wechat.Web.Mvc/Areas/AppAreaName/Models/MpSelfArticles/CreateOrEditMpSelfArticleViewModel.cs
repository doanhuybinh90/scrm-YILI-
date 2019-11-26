using Abp.AutoMapper;
using Pb.Wechat.MpSelfArticles.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpSelfArticles
{
    [AutoMapFrom(typeof(MpSelfArticleDto))]
    public class CreateOrEditMpSelfArticleViewModel : MpSelfArticleDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpSelfArticleViewModel(MpSelfArticleDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpSelfArticleViewModel() {
        }
    }
}
