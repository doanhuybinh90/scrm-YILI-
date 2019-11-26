using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.CustomerMediaVideos.Dto
{
    public class GetCustomerMediaVideosInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public string MediaID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id DESC";
            }
        }
    }
}
