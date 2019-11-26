using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerArticleGroupItems.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.CustomerArticleGroupItems
{
    public interface ICustomerArticleGroupItemAppService : IAsyncCrudAppService<CustomerArticleGroupItemDto, int, GetCustomerArticleGroupItemsInput, CustomerArticleGroupItemDto, CustomerArticleGroupItemDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerArticleGroupItemsInput input);
        Task<CustomerArticleGroupItemDto> GetModelByReplyTypeAsync(int id, int mpId);

        Task<bool> SaveItems(List<CustomerArticleGroupItemDto> inputs);
        Task<PagedResultDto<CustomerArticleGroupItemDto>> GetGroupItemList(EntityDto<int> GroupID);
        Task DeleteByGroupID(int groupId);
        Task<List<CustomerArticleGroupItemDto>> GetGroupItems(int groupId);
    }
}
