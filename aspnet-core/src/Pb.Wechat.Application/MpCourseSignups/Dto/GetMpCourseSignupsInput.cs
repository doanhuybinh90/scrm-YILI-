using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpCourseSignups.Dto
{
    public class GetMpCourseSignupsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public string OpenID { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public bool IsConfirmed { get; set; }
        public int CRMID { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
