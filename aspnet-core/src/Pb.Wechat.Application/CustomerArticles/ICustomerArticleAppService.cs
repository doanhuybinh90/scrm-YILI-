using Abp.Application.Services;
using Pb.Wechat.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.CustomerArticles.Dto.CustomerArticles
{
    public interface ICustomerArticleAppService : IAsyncCrudAppService<CustomerArticleDto, int, GetCustomerArticlesInput, CustomerArticleDto, CustomerArticleDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerArticlesInput input);
        Task<CustomerArticleDto> GetModelByReplyTypeAsync(int id, int mpId);
        Task<List<CustomerArticleDto>> GetListByIds(List<int> ids);
    }
}
