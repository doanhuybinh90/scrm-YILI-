using Abp;
using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpProductTypes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpProductTypes
{
    public interface IMpProductTypeAppService : IAsyncCrudAppService<MpProductTypeDto, int, GetMpProductTypesInput, MpProductTypeDto, MpProductTypeDto>
    {
        Task<FileDto> GetListToExcel(GetMpProductTypesInput input);
        Task<List<NameValue<string>>> GetTypes();
    }
}
