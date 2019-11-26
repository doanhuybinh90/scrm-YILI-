using Abp.AutoMapper;
using Pb.Wechat.CYProblemContents.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CYProblemContents
{
    [AutoMapFrom(typeof(CYProblemContentDto))]
    public class CreateOrEditCYProblemContentViewModel : CYProblemContentDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCYProblemContentViewModel(CYProblemContentDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCYProblemContentViewModel()
        {
        }
    }
}
