using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEventScanLogs.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpEventScanLogs
{
    public interface IMpEventScanLogAppService : IAsyncCrudAppService<MpEventScanLogDto, int, GetMpEventScanLogsInput, MpEventScanLogDto, MpEventScanLogDto>
    {
        Task<FileDto> GetListToExcel(GetMpEventScanLogsInput input);
        Task<MpEventScanLogDto> GetFirstOrDefault();
        
    }
}
