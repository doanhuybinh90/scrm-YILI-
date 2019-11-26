using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.TaskGroupMessages.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.TaskGroupMessages
{
    public interface ITaskGroupMessageAppService : IAsyncCrudAppService<TaskGroupMessageDto, long, GetTaskGroupMessagesInput, TaskGroupMessageDto, TaskGroupMessageDto>
    {
        Task<FileDto> GetListToExcel(GetTaskGroupMessagesInput input);

        Task<List<TaskGroupMessageDto>> GetList(GetTaskGroupMessagesInput input);
        Task SaveSuccessSendState(TaskGroupMessageDto input);

        Task<TaskGroupSendResult> GetSendResultAsync(int mpid, string taskId);

        Task<TaskGroupMessageDto> GetByWxMsgId(string id);
    }
}
