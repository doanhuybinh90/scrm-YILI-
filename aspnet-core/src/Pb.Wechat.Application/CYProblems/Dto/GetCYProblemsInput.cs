using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.CYProblems.Dto
{
    public class GetCYProblemsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Keyword { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id ASC";
            }
        }
    }
}
