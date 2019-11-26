using System.Collections.Generic;

namespace Pb.Wechat.MpGroups.Dto
{
    public class GetFansGroupInput 
    {
        public int GroupID { get; set; }
        public string GgroupName { get; set; }
        public int WxGroupID { get; set; }
        public List<int> FansIDs { get; set; }
    }
}
