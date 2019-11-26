namespace Pb.Wechat.MpUserMembers.Dto
{
    public class MemberAuthModal
    {
        public string OpenId { get; set; }
        public string MgccAuthKey { get; set; }
        public MpUserMember MemberInfo { get; set; }
    }
}
