using Abp.Dependency;
using System.Threading.Tasks;

namespace Pb.Wechat.MpAccessTokenClib
{
    public interface IAccessTokenContainer: ITransientDependency
    {
        Task<string> TryGetAccessTokenAsync(string appId, string appSecret, bool getNewToken = false);
        Task RegisterAsync(string appId, string appSecret, string name = null);
    }
}
