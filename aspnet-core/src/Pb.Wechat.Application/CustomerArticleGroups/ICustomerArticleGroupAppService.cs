using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerArticleGroups.Dto;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.CustomerArticleGroups
{
    public interface ICustomerArticleGroupAppService : IAsyncCrudAppService<CustomerArticleGroupDto, int, GetCustomerArticleGroupsInput, CustomerArticleGroupDto, CustomerArticleGroupDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerArticleGroupsInput input);
        Task<CustomerArticleGroupDto> GetModelByReplyTypeAsync(int id, int mpId);
        Task<bool> Save(CustomerArticleGroupDto input);
        Task<PagedResultDto<CustomerArticleGroupOutput>> GetMultiArticlesDataList(GetCustomerArticleGroupsInput input);
    }
}
