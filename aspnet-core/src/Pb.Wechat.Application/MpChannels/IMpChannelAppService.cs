using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpChannels.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpChannels
{
    public interface IMpChannelAppService : IAsyncCrudAppService<MpChannelDto, int, GetMpChannelsInput, MpChannelDto, MpChannelDto>
    {
        Task<FileDto> GetListToExcel(GetMpChannelsInput input);
        Task<MpChannelDto> GetFirstOrDefault(GetMpChannelsInput input);
        Task<PagedResultDto<MpChannelOutput>> GetChannelPage(GetMpChannelsInput input);
        Task<List<MpChannelDto>> GetAllChannel(GetMpChannelsInput input);
        Task ClearRegister();
        Task<List<NameValue<string>>> GetChannels();
    }
}
