using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class MpFanDto
    {
        public int Id { get; set; }
        public int MpID { get; set; }

        public string OpenID { get; set; }

        public string UnionID { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string HeadImgUrl { get; set; }
        public DateTime? SubscribeTime { get; set; }
        public DateTime? FirstSubscribeTime { get; set; }
        public string Remark { get; set; }
        public int? WxGroupID { get; set; }
        public int? GroupID { get; set; }
        public string GroupName { get; set; }
        public bool IsFans { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UnionName { get; set; }
        public int? ChannelID { get; set; }
        public string ChannelName { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int MemberID { get; set; }
        public string ChannelType { get; set; }
        public int LastCustomerServiceId { get; set; }
        public string LastCustomerServiceOpenId { get; set; }
    }
}