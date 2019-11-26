using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpApiTokens.Dto
{
    public class MpAccountTokenInput
    {
        
        public virtual int MpID { get; set; }
        [Required]
        public virtual string Token { get; set; }
        [Required]
        public virtual string ApiType { get; set; }
    }
}
