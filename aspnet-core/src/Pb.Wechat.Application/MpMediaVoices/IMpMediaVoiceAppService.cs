using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaVoices.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaVoices
{
    public interface IMpMediaVoiceAppService : IAsyncCrudAppService<MpMediaVoiceDto, int, GetMpMediaVoicesInput, MpMediaVoiceDto, MpMediaVoiceDto>
    {
        Task<FileDto> GetListToExcel(GetMpMediaVoicesInput input);
    }
}
