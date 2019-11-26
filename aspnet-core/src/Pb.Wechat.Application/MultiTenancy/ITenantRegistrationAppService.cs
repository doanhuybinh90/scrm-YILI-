using System.Threading.Tasks;
using Abp.Application.Services;
using Pb.Wechat.Editions.Dto;
using Pb.Wechat.MultiTenancy.Dto;

namespace Pb.Wechat.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}