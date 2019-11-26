using System.Collections.Generic;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpGroups
{
    public class GroupSelectModal
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<GroupSelectModal> children { get; set; }
    }
}
