using System.Collections.Generic;

namespace Pb.Wechat.MpFans.Dto
{
    public class MpFanGroupDto
    {
        public int GroupID { get; set; }
        public List<int> FansIds { get; set; }
        public string GroupName { get; set; }
    }
}
