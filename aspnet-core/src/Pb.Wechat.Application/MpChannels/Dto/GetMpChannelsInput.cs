using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using System;

namespace Pb.Wechat.MpChannels.Dto
{
    public class GetMpChannelsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public string Title { get; set; }
        public string EventKey { get; set; }
        public DateTime? CreationStartTime { get; set; }
        public DateTime? CreationEndTime { get; set; }
        public ChannelType? ChannelType { get; set; }
        public DateTime? ValidityStartTime { get; set; }
        public DateTime? ValidityEndTime { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
