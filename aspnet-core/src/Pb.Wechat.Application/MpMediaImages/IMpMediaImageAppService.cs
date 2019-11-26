using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaImages.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaImages
{
    public interface IMpMediaImageAppService : IAsyncCrudAppService<MpMediaImageDto, int, GetMpMediaImagesInput, MpMediaImageDto, MpMediaImageDto>
    {
        Task<FileDto> GetListToExcel(GetMpMediaImagesInput input);
        Task<MpMediaImageDto> GetByFileID(string fileID);
        Task<MpMediaImageDto> GetByMediaID(string mediaID);

    }
}
