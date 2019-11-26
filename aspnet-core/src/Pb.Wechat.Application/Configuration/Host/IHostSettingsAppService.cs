using System.Threading.Tasks;
using Abp.Application.Services;
using Pb.Wechat.Configuration.Host.Dto;

namespace Pb.Wechat.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
