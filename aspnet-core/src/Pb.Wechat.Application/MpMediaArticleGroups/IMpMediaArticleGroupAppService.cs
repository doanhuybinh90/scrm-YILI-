using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaArticleGroups.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaArticleGroups
{
    public interface IMpMediaArticleGroupAppService : IAsyncCrudAppService<MpMediaArticleGroupDto, int, GetMpMediaArticleGroupsInput, MpMediaArticleGroupDto, MpMediaArticleGroupDto>
    {
        Task<FileDto> GetListToExcel(GetMpMediaArticleGroupsInput input);
        Task<MpMediaArticleGroupDto> GetModelByReplyTypeAsync(int id, int mpId);

        /// <summary>
        /// 保存群发多图文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MpMediaArticleGroupDto> Save(SaveMpMediaArticleGroupInput input);

        /// <summary>
        /// 获取字表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PagedResultDto<MpMediaArticleGroupItemDto>> GetGroupItemList(EntityDto<int> input);

        Task<List<MpMediaArticleGroupItemDto>> GetGroupItemListByGroupIds(List<int?> inputs);
        Task<PagedResultDto<MediaArticleGroupOutput>> GetMultiArticlesDataList(GetMpMediaArticleGroupsInput input);
    }
}
