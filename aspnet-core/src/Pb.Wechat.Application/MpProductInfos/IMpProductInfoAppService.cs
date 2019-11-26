using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpProductInfos.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpProductInfos
{
    public interface IMpProductInfoAppService : IAsyncCrudAppService<MpProductInfoDto, int, GetMpProductInfosInput, MpProductInfoDto, MpProductInfoDto>
    {
        Task<FileDto> GetListToExcel(GetMpProductInfosInput input);

        Task<MpProductInfoDto> GetFirstOrDefaultByInput(GetMpProductInfosInput input);
        Task<List<MpProductListDto>> GetList();
    }
}
