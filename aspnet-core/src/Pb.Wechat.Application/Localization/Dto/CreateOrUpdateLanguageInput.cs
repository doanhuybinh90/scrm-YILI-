using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}