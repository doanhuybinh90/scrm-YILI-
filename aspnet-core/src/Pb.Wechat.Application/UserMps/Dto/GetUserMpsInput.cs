using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.UserMps.Dto
{
    public class GetUserMpsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long? UserId { get; set; }
        public int CurrentMpID { get; set; }

       
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id ASC";
            }
            
        }
    }
}
