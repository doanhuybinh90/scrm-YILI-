using System.Threading.Tasks;
using Abp.Application.Services;
using Pb.Wechat.Configuration.Tenants.Dto;

namespace Pb.Wechat.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
