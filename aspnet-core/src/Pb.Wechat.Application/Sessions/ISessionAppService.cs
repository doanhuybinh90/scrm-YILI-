using System.Threading.Tasks;
using Abp.Application.Services;
using Pb.Wechat.Sessions.Dto;

namespace Pb.Wechat.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
