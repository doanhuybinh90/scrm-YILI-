using Pb.Wechat.MpGroups.Dto;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpFans
{
    public class MpFanViewModel
    {
        public int MpID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string NickName { get; set; }
        public string FansState { get; set; }
        public string UnionName { get; set; }
        public IEnumerable<MpGroupDto> Groups { get; set; }
        public string ChannelType { get; set; }
        public int MemberID { get; set; }
        public string Token { get; set; }
    }
}
