using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Pb.Wechat.CustomerServiceConversations.Exporting;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Runtime.Caching;
using Pb.Wechat.CustomerServiceConversationMsgs;
using System;
using Abp.Application.Services.Dto;
using Pb.Wechat.CustomerServiceOnlines;
using Pb.Wechat.CustomerServiceOnlines.Dto;

namespace Pb.Wechat.CustomerServiceConversations
{
    public class CustomerServiceConversationAppService : AsyncCrudAppService<CustomerServiceConversation, CustomerServiceConversationDto, long, GetCustomerServiceConversationsInput, CustomerServiceConversationDto, CustomerServiceConversationDto>, ICustomerServiceConversationAppService
    {
        private readonly ICustomerServiceConversationListExcelExporter _customerServiceConversationListExcelExporter;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<CustomerServiceConversationMsg, long> _cusMsgRepository;
        private readonly IRepository<CustomerServiceOnline, int> _cusInfos;

        public CustomerServiceConversationAppService(IRepository<CustomerServiceConversation, long> repository, ICustomerServiceConversationListExcelExporter customerServiceConversationListExcelExporter, ICacheManager cacheManager, IRepository<CustomerServiceConversationMsg, long> cusMsgRepository, IRepository<CustomerServiceOnline, int> cusInfos) : base(repository)
        {
            _customerServiceConversationListExcelExporter = customerServiceConversationListExcelExporter;
            _cacheManager = cacheManager;
            _cusMsgRepository = cusMsgRepository;
            _cusInfos = cusInfos;
        }

        protected override IQueryable<CustomerServiceConversation> CreateFilteredQuery(GetCustomerServiceConversationsInput input)
        {
            List<int> cusIds = null;
            var kfType = KFType.YL.ToString();
            if (!string.IsNullOrWhiteSpace(input.KfNickName))
            {
                cusIds = _cusInfos.GetAllList(m => m.IsDeleted == false && m.KfType == kfType && m.MpID == input.MpID && m.KfNick.Contains(input.KfNickName)).Select(m => m.Id).ToList();
            }
            return Repository.GetAll()
                .WhereIf(input.MpID!=0,c=>c.MpID==input.MpID)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.FanOpenId.Contains(input.Keyword))
                .WhereIf(input.StartTime.HasValue, c => c.StartTalkTime >= input.StartTime.Value)
                .WhereIf(input.EndTime.HasValue, c => c.StartTalkTime <= input.EndTime.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(input.FanNickName), c => c.NickName.Contains(input.FanNickName))
                .WhereIf(cusIds != null && cusIds.Count > 0, c => cusIds.Contains((int)c.CustomerId));
        }
        public async Task<FileDto> GetListToExcel(GetCustomerServiceConversationsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _customerServiceConversationListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CustomerServiceConversationDto> GetUserLastConversation(string openid)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(c => c.FanOpenId == openid).OrderByDescending(c => c.CreationTime)));
        }
        public async Task<List<string>> CloseConversations(int mpid)
        {
            List<string> openIds = null;
            var state = (int)CustomerServiceConversationState.Asking;
            var allAskingList = Repository.GetAll().Where(m => m.State == state && m.MpID == mpid).ToList();
            var removeList = new List<CustomerServiceConversation>();
            foreach (var convesation in allAskingList)
            {
                var lastMsg = _cusMsgRepository.GetAll().OrderByDescending(m => m.CreationTime).FirstOrDefault();
                if (DateTime.Now > lastMsg.CreationTime.AddMinutes(30))
                {
                    if (openIds == null)
                        openIds = new List<string>();
                    openIds.Add(convesation.FanOpenId);
                    removeList.Add(convesation);
                }

            }
            foreach (var item in removeList)
            {
                await Repository.UpdateAsync(item.Id, async m =>
                {
                    m.State = (int)CustomerServiceConversationState.Closed;

                });
            }
            return openIds;
        }

        public async Task<List<CustomerServiceConversationDto>> GetAllAskingList(int mpid, int state)
        {

            return ObjectMapper.Map<List<CustomerServiceConversationDto>>(await Repository.GetAllListAsync(m => m.MpID == mpid && m.State == state));
        }

        public async Task<PagedResultDto<CustomerConversationOutput>> GetConversationList(GetCustomerServiceConversationsInput input)
        {
            CheckGetAllPermission();
            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);

            List<int> cusIds = null;
            var kfType = KFType.YL.ToString();
            if (!string.IsNullOrWhiteSpace(input.KfNickName))
            {
                cusIds = _cusInfos.GetAllList(m => m.IsDeleted == false && m.KfType == kfType && m.MpID == input.MpID).Select(m => m.Id).ToList();
            }

            List<int> fanIds = null;
            if (!string.IsNullOrWhiteSpace(input.FanNickName))
            {
                cusIds = _cusInfos.GetAllList(m => m.IsDeleted == false && m.KfType == kfType && m.MpID == input.MpID).Select(m => m.Id).ToList();
            }
            var fanSender = (int)CustomerServiceMsgSender.user;
            var cusSender = (int)CustomerServiceMsgSender.customer;
            var msgQuery = _cusMsgRepository.GetAll().WhereIf(input.StartTime.HasValue, m => m.CreationTime >= input.StartTime.Value).WhereIf(input.EndTime.HasValue, m => m.CreationTime <= input.EndTime.Value).WhereIf(cusIds != null && cusIds.Count > 0, m => cusIds.Contains((int)m.CustomerId)).GroupBy(m => new { m.ConversationId }).Select(m => new
            {
                m.Key.ConversationId,
                fanMsgCount = m.Count(t => t.Sender == fanSender),
                cusMsgCount = m.Count(t => t.Sender == cusSender)
            });
            var kfInfo = _cusInfos.GetAll().Where(m => m.KfType == kfType);
            var resultQuery = from t in query
                     join tt in msgQuery on t.Id equals tt.ConversationId
                     join tm in kfInfo on t.CustomerId equals tm.Id
                     select new CustomerConversationOutput
                     {
                         Id = t.Id,
                         State = t.State,
                         KfOpenId = t.FanOpenId,
                         StartTalkTime = t.StartTalkTime,
                         EndTalkTime = t.EndTalkTime,
                         FanId = t.FanId,
                         CustomerMsgCount = tt.cusMsgCount,
                         FanHeadImgUrl = t.HeadImgUrl,
                         FanMsgCount = tt.fanMsgCount,
                         FanNickName = t.NickName,
                         FanOpenId = t.FanOpenId,
                         ConversationScore = t.ConversationScore,
                         KfNickName=tm.KfNick
                     };
            var totalCount = resultQuery.Count();

            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                resultQuery = resultQuery.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
            }
            var resultList = resultQuery.ToList();
            return new PagedResultDto<CustomerConversationOutput>
            {
                TotalCount = totalCount,
                Items = resultList
            };
     
        }
    }
}
