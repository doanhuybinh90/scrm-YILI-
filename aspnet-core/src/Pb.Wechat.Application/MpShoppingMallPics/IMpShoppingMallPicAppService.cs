using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpShoppingMallPics.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpShoppingMallPics
{
    public interface IMpShoppingMallPicAppService : IAsyncCrudAppService<MpShoppingMallPicDto, int, GetMpShoppingMallPicsInput, MpShoppingMallPicDto, MpShoppingMallPicDto>
    {
        Task<FileDto> GetListToExcel(GetMpShoppingMallPicsInput input);

        Task<MpShoppingMallPicDto> GetFirstOrDefaultByInput(GetMpShoppingMallPicsInput input);
        Task<List<MpShoppingMallPicDto>> GetListByNames(string token, params string[] name);
    }
}
