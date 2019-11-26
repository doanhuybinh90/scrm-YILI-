using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}