using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpEvents
{
    public interface IMpEventAppService : IAsyncCrudAppService<MpEventDto, int, GetMpEventsInput, MpEventDto, MpEventDto>
    {
        Task<FileDto> GetListToExcel(GetMpEventsInput input);
        Task<MpEventDto> GetModelByReplyTypeAsync(string replyType, int mpId);

        Task<MpEventDto> GetModelByEventTypeAsync(string eventType, int mpId);
    }
}
