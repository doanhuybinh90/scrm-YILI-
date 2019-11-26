using Abp.Application.Services;
using Pb.Wechat.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSelfArticles.Dto.MpSelfArticles
{
    public interface IMpSelfArticleAppService : IAsyncCrudAppService<MpSelfArticleDto, int, GetMpSelfArticlesInput, MpSelfArticleDto, MpSelfArticleDto>
    {
        Task<FileDto> GetListToExcel(GetMpSelfArticlesInput input);
        Task<MpSelfArticleDto> GetModelByReplyTypeAsync(int id, int mpId);
        Task<List<MpSelfArticleDto>> GetListByIds(List<int> ids);
    }
}
