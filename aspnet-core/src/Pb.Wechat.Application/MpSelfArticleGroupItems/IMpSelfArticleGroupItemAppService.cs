using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSelfArticleGroupItems.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSelfArticleGroupItems
{
    public interface IMpSelfArticleGroupItemAppService : IAsyncCrudAppService<MpSelfArticleGroupItemDto, int, GetMpSelfArticleGroupItemsInput, MpSelfArticleGroupItemDto, MpSelfArticleGroupItemDto>
    {
        Task<FileDto> GetListToExcel(GetMpSelfArticleGroupItemsInput input);
        Task<MpSelfArticleGroupItemDto> GetModelByReplyTypeAsync(int id, int mpId);

        Task<bool> SaveItems(List<MpSelfArticleGroupItemDto> inputs);
        Task<PagedResultDto<MpSelfArticleGroupItemDto>> GetGroupItemList(EntityDto<int> GroupID);
        Task DeleteByGroupID(int groupId);
        Task<List<MpSelfArticleGroupItemDto>> GetGroupItems(int groupId);
    }
}
