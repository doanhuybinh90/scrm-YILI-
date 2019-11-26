using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSelfArticleGroups.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSelfArticleGroups
{
    public interface IMpSelfArticleGroupAppService : IAsyncCrudAppService<MpSelfArticleGroupDto, int, GetMpSelfArticleGroupsInput, MpSelfArticleGroupDto, MpSelfArticleGroupDto>
    {
        Task<FileDto> GetListToExcel(GetMpSelfArticleGroupsInput input);
        Task<MpSelfArticleGroupDto> GetModelByReplyTypeAsync(int id, int mpId);

        Task<bool> Save(MpSelfArticleGroupDto input);
        Task<PagedResultDto<MpSelfArticleGroupOutput>> GetMultiArticlesDataList(GetMpSelfArticleGroupsInput input);
    }
}
