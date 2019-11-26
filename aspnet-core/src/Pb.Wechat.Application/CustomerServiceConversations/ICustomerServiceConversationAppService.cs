using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceConversations.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.CustomerServiceConversations
{
    public interface ICustomerServiceConversationAppService : IAsyncCrudAppService<CustomerServiceConversationDto, long, GetCustomerServiceConversationsInput, CustomerServiceConversationDto, CustomerServiceConversationDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerServiceConversationsInput input);

        Task<CustomerServiceConversationDto> GetUserLastConversation(string openid);
        Task<List<string>> CloseConversations(int mpid);
        Task<List<CustomerServiceConversationDto>> GetAllAskingList(int mpid, int state);
        Task<PagedResultDto<CustomerConversationOutput>> GetConversationList(GetCustomerServiceConversationsInput input);
    }
}
