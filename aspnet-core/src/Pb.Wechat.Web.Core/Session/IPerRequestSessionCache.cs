using System.Threading.Tasks;
using Pb.Wechat.Sessions.Dto;

namespace Pb.Wechat.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
