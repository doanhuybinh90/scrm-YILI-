using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSolicitudeSettings.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSolicitudeSettings
{
    public interface IMpSolicitudeSettingAppService : IAsyncCrudAppService<MpSolicitudeSettingDto, int, GetMpSolicitudeSettingsInput, MpSolicitudeSettingDto, MpSolicitudeSettingDto>
    {
        Task<FileDto> GetListToExcel(GetMpSolicitudeSettingsInput input);

        Task<MpSolicitudeSettingDto> GetDefault();
    }
}
