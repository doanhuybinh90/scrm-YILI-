using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpFansTags.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpFansTags
{
    public interface IMpFansTagAppService : IAsyncCrudAppService<MpFansTagDto, int, GetMpFansTagsInput, MpFansTagDto, MpFansTagDto>
    {
        Task<FileDto> GetListToExcel(GetMpFansTagsInput input);
        Task<IList<MpFansTagDto>> GetAllTags(GetMpFansTagsInput input);
    }
}
