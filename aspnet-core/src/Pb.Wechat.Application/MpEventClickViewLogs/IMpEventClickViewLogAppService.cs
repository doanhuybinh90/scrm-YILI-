using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEventClickViewLogs.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpEventClickViewLogs
{
    public interface IMpEventClickViewLogAppService : IAsyncCrudAppService<MpEventClickViewLogDto, int, GetMpEventClickViewLogsInput, MpEventClickViewLogDto, MpEventClickViewLogDto>
    {
        Task<FileDto> GetListToExcel(GetMpEventClickViewLogsInput input);
        Task<MpEventClickViewLogDto> GetFirstOrDefault();
        
    }
}
