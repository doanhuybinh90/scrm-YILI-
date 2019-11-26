using System.Threading.Tasks;

namespace Pb.Wechat.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}