using System.ComponentModel.DataAnnotations;
using Pb.Wechat.Authorization.Users;

namespace Pb.Wechat.Configuration.Host.Dto
{
    public class SendTestEmailInput
    {
        [Required]
        [MaxLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}