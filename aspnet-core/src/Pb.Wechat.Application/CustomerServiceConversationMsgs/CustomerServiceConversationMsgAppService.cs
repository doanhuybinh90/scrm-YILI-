using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceConversationMsgs.Dto;
using Pb.Wechat.CustomerServiceConversationMsgs.Exporting;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.CustomerServiceConversationMsgs
{
    public class CustomerServiceConversationMsgAppService : AsyncCrudAppService<CustomerServiceConversationMsg, CustomerServiceConversationMsgDto, long, GetCustomerServiceConversationMsgsInput, CustomerServiceConversationMsgDto, CustomerServiceConversationMsgDto>, ICustomerServiceConversationMsgAppService
    {
        private readonly ICustomerServiceConversationMsgListExcelExporter _customerServiceConversationMsgListExcelExporter;
        public CustomerServiceConversationMsgAppService(IRepository<CustomerServiceConversationMsg, long> repository, ICustomerServiceConversationMsgListExcelExporter customerServiceConversationMsgListExcelExporter) : base(repository)
        {
            _customerServiceConversationMsgListExcelExporter = customerServiceConversationMsgListExcelExporter;
        }

        protected override IQueryable<CustomerServiceConversationMsg> CreateFilteredQuery(GetCustomerServiceConversationMsgsInput input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.MsgContent.Contains(input.Keyword));
        }
        public async Task<FileDto> GetListToExcel(GetCustomerServiceConversationMsgsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _customerServiceConversationMsgListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CustomerServiceConversationMsgDto> GetLastMsgDtoByFanID(int mpid,int fansId)
        {
            return ObjectMapper.Map<CustomerServiceConversationMsgDto>(Repository.GetAll().Where(m => m.MpID == mpid && m.FanId == fansId).OrderByDescending(m => m.CreationTime).FirstOrDefault());
        }
    }
}
