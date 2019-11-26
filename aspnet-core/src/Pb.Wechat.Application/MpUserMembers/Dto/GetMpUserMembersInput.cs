using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using System;

namespace Pb.Wechat.MpUserMembers.Dto
{
    public class GetMpUserMembersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string OpenID { get; set; }

        public string MobilePhone { get; set; }

        public string MemberPassword { get; set; }
        public int? ChannelID { get; set; }
        public string MemberUserName { get; set; }
        public bool? IsBinding { get; set; }
        public DateTime? CreationStartTime { get; set; }
        public DateTime? CreationEndTime { get; set; }
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
