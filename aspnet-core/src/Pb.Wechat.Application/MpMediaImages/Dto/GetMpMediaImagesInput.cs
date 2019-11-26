using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using System;

namespace Pb.Wechat.MpMediaImages.Dto
{
    public class GetMpMediaImagesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }

        public string MediaID { get; set; }

        public string Title { get; set; }

        public string FileID { get; set; }
        public string FilePathOrUrl { get; set; }
        public DateTime? CreationStartTime { get; set; }
        public DateTime? CreationEndTime { get; set; }
        public int? MediaImageType { get; set; }
        public string MediaImageTypeName { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id DESC";
            }
            
        }
    }
}
