using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEventRequestMsgLogs.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpEventRequestMsgLogs
{
    public interface IMpEventRequestMsgLogAppService : IAsyncCrudAppService<MpEventRequestMsgLogDto, int, GetMpEventRequestMsgLogsInput, MpEventRequestMsgLogDto, MpEventRequestMsgLogDto>
    {
        Task<FileDto> GetListToExcel(GetMpEventRequestMsgLogsInput input);
        Task<MpEventRequestMsgLogDto> GetFirstOrDefault();
        
    }
}
