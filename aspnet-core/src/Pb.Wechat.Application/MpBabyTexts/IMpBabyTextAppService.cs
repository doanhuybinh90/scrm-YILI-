using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpBabyTexts.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpBabyTexts
{
    public interface IMpBabyTextAppService : IAsyncCrudAppService<MpBabyTextDto, int, GetMpBabyTextsInput, MpBabyTextDto, MpBabyTextDto>
    {
        Task<FileDto> GetListToExcel(GetMpBabyTextsInput input);

        Task<MpBabyTextDto> GetFirstOrDefaultByInput(GetMpBabyTextsInput input);
        Task<MpBabyTextDto> GetMaxWeekText(int week);
        Task<MpBabyTextDto> GetTextByday(int day);
    }
}
