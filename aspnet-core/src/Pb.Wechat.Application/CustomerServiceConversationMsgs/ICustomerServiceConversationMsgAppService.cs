using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceConversationMsgs.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerServiceConversationMsgs
{
    public interface ICustomerServiceConversationMsgAppService : IAsyncCrudAppService<CustomerServiceConversationMsgDto, long, GetCustomerServiceConversationMsgsInput, CustomerServiceConversationMsgDto, CustomerServiceConversationMsgDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerServiceConversationMsgsInput input);
        Task<CustomerServiceConversationMsgDto> GetLastMsgDtoByFanID(int mpid, int fansId);
    }
}
