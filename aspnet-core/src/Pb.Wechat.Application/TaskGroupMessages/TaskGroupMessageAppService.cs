using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.TaskGroupMessages.Dto;
using Pb.Wechat.UserMps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.TaskGroupMessages
{
    //[AbpAuthorize(AppPermissions.Pages_TaskGroupMessages)]
    public class TaskGroupMessageAppService : AsyncCrudAppService<TaskGroupMessage, TaskGroupMessageDto, long, GetTaskGroupMessagesInput, TaskGroupMessageDto, TaskGroupMessageDto>, ITaskGroupMessageAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly ITaskGroupMessageListExcelExporter _TaskGroupMessageListExcelExporter;
        public TaskGroupMessageAppService(IRepository<TaskGroupMessage, long> repository, ITaskGroupMessageListExcelExporter TaskGroupMessageListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _TaskGroupMessageListExcelExporter = TaskGroupMessageListExcelExporter;
            _userMpAppService = userMpAppService;


        }

        protected override IQueryable<TaskGroupMessage> CreateFilteredQuery(GetTaskGroupMessagesInput input)
        {
            return Repository.GetAll()
                .WhereIf(input.MpID.HasValue, c => c.MpID == input.MpID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TaskID), c => c.TaskID.Contains(input.TaskID))
                .WhereIf(input.MessageId.HasValue, c => c.MessageID== input.MessageId)
                .WhereIf(input.TaskState.HasValue, c => c.TaskState == input.TaskState)
               ;
        }

        public async Task<FileDto> GetListToExcel(GetTaskGroupMessagesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _TaskGroupMessageListExcelExporter.ExportToFile(dtos);
        }

        public async Task<List<TaskGroupMessageDto>> GetList(GetTaskGroupMessagesInput input)
        {
            return (await AsyncQueryableExecuter.ToListAsync(CreateFilteredQuery(input))).Select(MapToEntityDto).ToList();
        }

        public async Task SaveSuccessSendState(TaskGroupMessageDto input)
        {
            await Repository.InsertOrUpdateAsync(MapToEntity(input));
        }
        public async Task<TaskGroupSendResult> GetSendResultAsync(int mpid, string taskId)
        {
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.IsDeleted == false && m.TaskID == taskId && m.MpID == mpid))).GroupBy(m => new { m.TaskID, m.MpID }).Select(m => new TaskGroupSendResult
            {
                TaskID = m.Key.TaskID,
                MpID = m.Key.MpID,
                FailCount = m.Sum(t => t.FailCount),
                SuccessCount = m.Sum(t => t.SuccessCount),
                SendCount = m.Sum(t => t.SendCount)
            }).FirstOrDefault();
        }
        public async Task<TaskGroupMessageDto> GetByWxMsgId(string id) {
            return MapToEntityDto(await Repository.FirstOrDefaultAsync(c => c.WxMsgID == id));
        }
    }
}
