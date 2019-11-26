using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpMediaVoices.Dto
{
    public class GetMpMediaVoicesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }

        public string MediaID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string FileID { get; set; }
        public string FilePathOrUrl { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id DESC";
            }
            
        }
    }
}
