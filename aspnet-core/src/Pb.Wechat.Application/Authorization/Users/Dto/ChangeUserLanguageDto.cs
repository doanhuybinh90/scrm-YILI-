using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
