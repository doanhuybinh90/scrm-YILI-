using Pb.Wechat.MpGroups.Dto;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpGroups
{
    public class MpGroupViewModel
    {
        public int MpID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string NickName { get; set; }
        public string GroupsState { get; set; }
        public string UnionName { get; set; }
        public IEnumerable<MpGroupDto> Groups { get; set; }
    }
}
