using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaVideos.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaVideos
{
    public interface IMpMediaVideoAppService : IAsyncCrudAppService<MpMediaVideoDto, int, GetMpMediaVideosInput, MpMediaVideoDto, MpMediaVideoDto>
    {
        Task<FileDto> GetListToExcel(GetMpMediaVideosInput input);
        Task<MpMediaVideoDto> GetModelByReplyTypeAsync(string mediaId, int mpId);
    }
}
