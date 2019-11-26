using System.Threading.Tasks;

namespace Pb.Wechat.Identity
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}