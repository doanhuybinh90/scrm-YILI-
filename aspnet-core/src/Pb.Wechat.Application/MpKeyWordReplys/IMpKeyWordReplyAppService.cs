using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpKeyWordReplys.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpKeyWordReplys
{
    public interface IMpKeyWordReplyAppService : IAsyncCrudAppService<MpKeyWordReplyDto, int, GetMpKeyWordReplysInput, MpKeyWordReplyDto, MpKeyWordReplyDto>
    {
        Task<FileDto> GetListToExcel(GetMpKeyWordReplysInput input);
        Task<MpKeyWordReplyDto> GetModelByReplyTypeAsync(string replyType, int mpId);

        Task<MpKeyWordReplyDto> GetEntityByKeyWordAsync(string content, int mpId);
        Task<PagedResultDto<MpKeyWordOutput>> GetKeywordsPage(GetMpKeyWordReplysInput input);
    }
}
