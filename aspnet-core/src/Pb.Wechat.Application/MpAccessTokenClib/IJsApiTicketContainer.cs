using Abp.Dependency;
using System.Threading.Tasks;

namespace Pb.Wechat.MpAccessTokenClib
{
    public interface IJsApiTicketContainer: ITransientDependency
    {
        Task RegisterAsync(string appId, string appSecret, string name = null);
        Task<string> TryGetJsApiTicketAsync(string appId, string appSecret, bool getNewTicket = false);
    }
}
