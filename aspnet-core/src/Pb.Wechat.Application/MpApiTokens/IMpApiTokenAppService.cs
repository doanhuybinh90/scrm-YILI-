using Abp.Application.Services;
using Pb.Wechat.MpApiTokens.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpApiTokens
{
    public interface IMpApiTokenAppService : IAsyncCrudAppService<MpApiTokenDto, int, GetMpApiTokensInput, MpApiTokenDto, MpApiTokenDto>
    {
        Task<MpAccountTokenOutput> GetAccountToken(MpAccountTokenInput input);
    }
}
