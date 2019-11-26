using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSecondKeyWordReplys.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSecondKeyWordReplys
{
    public interface IMpSecondKeyWordReplyAppService : IAsyncCrudAppService<MpSecondKeyWordReplyDto, int, GetMpSecondKeyWordReplysInput, MpSecondKeyWordReplyDto, MpSecondKeyWordReplyDto>
    {
        Task<FileDto> GetListToExcel(GetMpSecondKeyWordReplysInput input);
        Task<MpSecondKeyWordReplyDto> GetEntityByKeyWordAsync(string content, int parentId);
        Task<PagedResultDto<MpSecondKeyWordOutput>> GetKeywordsPage(GetMpSecondKeyWordReplysInput input);
    }
}
