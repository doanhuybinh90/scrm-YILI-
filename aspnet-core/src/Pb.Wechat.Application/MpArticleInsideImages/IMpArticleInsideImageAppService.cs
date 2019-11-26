using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpArticleInsideImages.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpArticleInsideImages
{
    public interface IMpArticleInsideImageAppService : IAsyncCrudAppService<MpArticleInsideImageDto, int, GetMpArticleInsideImagesInput, MpArticleInsideImageDto, MpArticleInsideImageDto>
    {
        Task<FileDto> GetListToExcel(GetMpArticleInsideImagesInput input);
        Task<MpArticleInsideImageDto> GetFirstOrDefault(GetMpArticleInsideImagesInput input);
        Task<List<MpArticleInsideImageDto>> GetList(GetMpArticleInsideImagesInput input);
    }
}
