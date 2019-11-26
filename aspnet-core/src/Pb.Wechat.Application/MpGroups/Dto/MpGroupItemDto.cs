using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Pb.Wechat.MpGroupItems;
using System;

namespace Pb.Wechat.MpGroups.Dto
{
    [AutoMap(typeof(MpGroupItem))]
    public class MpGroupItemDto : EntityDto<int>
    {
        public string IsMember { get; set; }
        public int MpID { get; set; }
  
        public int ParentID { get; set; }

        public string Name { get; set; }
        public int BaySex { get; set; }

        public string OrganizeCity { get; set; }
       
        public string OfficialCity { get; set; }
      
        public string LastBuyProduct { get; set; }
    
        public string MemberCategory { get; set; }
 
        public DateTime BeginBabyBirthday { get; set; }

        public DateTime EndBabyBirthday { get; set; }
  
        public int BeginPointsBalance { get; set; }
  
        public int EndPointsBalance { get; set; }

        public string ChannelID { get; set; }
   
        public string ChannelName { get; set; }
    
        public string TargetID { get; set; }

        public string TargetName { get; set; }
        public string MotherType { get; set; }

    }
}
