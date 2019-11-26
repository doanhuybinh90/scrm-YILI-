using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaArticles.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaArticles
{
    public interface IMpMediaArticleAppService : IAsyncCrudAppService<MpMediaArticleDto, int, GetMpMediaArticlesInput, MpMediaArticleDto, MpMediaArticleDto>
    {
        Task<FileDto> GetListToExcel(GetMpMediaArticlesInput input);
        Task<List<MpMediaArticleDto>> GetList();
        Task Save(MpMediaArticleDto input);
        Task<List<MpMediaArticleDto>> GetListByIds(List<int> Ids);
        Task<string> UpdateWxContentString(string content, string articleGuid,string domain=null);
    }
}
