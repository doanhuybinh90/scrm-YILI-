using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using System;

namespace Pb.Wechat.MpMediaImageTypes.Dto
{
    public class GetMpMediaImageTypesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public string MediaTypeName { get; set; }
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
