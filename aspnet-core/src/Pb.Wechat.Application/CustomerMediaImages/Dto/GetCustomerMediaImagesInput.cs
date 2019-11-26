using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using System;

namespace Pb.Wechat.CustomerMediaImages.Dto
{
    public class GetCustomerMediaImagesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }

        public string MediaID { get; set; }

        public string Title { get; set; }

        public string FilePathOrUrl { get; set; }
        public DateTime? CreationStartTime { get; set; }
        public DateTime? CreationEndTime { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id DESC";
            }
            
        }
    }
}
