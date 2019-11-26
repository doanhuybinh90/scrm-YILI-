using Abp.AutoMapper;
using Pb.Wechat.CYProblems.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CYProblems
{
    [AutoMapFrom(typeof(CYProblemDto))]
    public class CreateOrEditCYProblemViewModel : CYProblemDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCYProblemViewModel(CYProblemDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCYProblemViewModel()
        {
        }
    }
}
