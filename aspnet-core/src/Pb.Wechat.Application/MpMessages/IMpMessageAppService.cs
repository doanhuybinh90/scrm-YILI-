using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMessages.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMessages
{
    public interface IMpMessageAppService : IAsyncCrudAppService<MpMessageDto, int, GetMpMessagesInput, MpMessageDto, MpMessageDto>
    {
        Task<FileDto> GetListToExcel(GetMpMessagesInput input);

        Task<MpMessageDto> GetModelByMessageTypeAsync(string MessageType, int mpId);
        Task<List<MpMessageDto>> GetNotYetSendContent();
        Task UpdateSendState(List<MpMessageDto> inputs);
        Task<MpMessageDto> GetById(int id);
        Task<MpMessageDto> GetFirstOrDefault(GetMpMessagesInput input);
        Task<MpMessageDto> TaskUpdate(MpMessageDto input);
    }
}
