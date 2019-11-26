using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}