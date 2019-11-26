using System;

namespace Pb.Wechat.MpApiTokens.Dto
{
    public class MpAccountTokenOutput
    {
        public virtual int MpId { get; set; }
        public virtual string AppId { get; set; }
        public virtual string AppSecret { get; set; }
        public virtual string Domain { get; set; }
        public virtual string MchID { get; set; }
        public virtual string WxPayAppSecret { get; set; }
        public virtual string CertPhysicalPath { get; set; }
        public virtual string CertPassword { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
    }
}
