using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using Pb.Wechat.MpChannels.Dto;
using System;

namespace Pb.Wechat.MpFans.Dto
{
    public class GetMpFansInput : PagedAndSortedInputDto, IShouldNormalize
    {

        public int? MpID { get; set; }

        public string OpenID { get; set; }

        public string UnionName { get; set; }
        public string NickName { get; set; }

        public int GroupID { get; set; }
        public DateTime? SubscribeStartDate { get; set; }
        public DateTime? SubscribeEndDate { get; set; }
        public int? ChannelID { get; set; }
        public string ChannelName { get; set; }
        public bool? IsMember { get; set; }
        public string ChannelIDs { get; set; }
        public ChannelEnum? ChannelType { get; set; }
        public bool? IsFans { get; set; }
        public string OpenIDs { get; set; }
        public bool? GroupSearch { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
            
        }
    }
}
