using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSolicitudeTexts.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSolicitudeTexts
{
    public interface IMpSolicitudeTextAppService : IAsyncCrudAppService<MpSolicitudeTextDto, int, GetMpSolicitudeTextsInput, MpSolicitudeTextDto, MpSolicitudeTextDto>
    {
        Task<FileDto> GetListToExcel(GetMpSolicitudeTextsInput input);

        Task<MpSolicitudeTextDto> GetFirstOrDefaultByInput(GetMpSolicitudeTextsInput input);
        Task<MpSolicitudeTextDto> GetMaxWeekText(int week);
        Task<MpSolicitudeTextDto> GetTextByday(int day);
    }
}
