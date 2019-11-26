using Abp;
using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaImageTypes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaImageTypes
{
    public interface IMpMediaImageTypeAppService : IAsyncCrudAppService<MpMediaImageTypeDto, int, GetMpMediaImageTypesInput, MpMediaImageTypeDto, MpMediaImageTypeDto>
    {
        Task<FileDto> GetListToExcel(GetMpMediaImageTypesInput input);
        Task<List<NameValue<string>>> GetAllList(GetMpMediaImageTypesInput input);
    }
}
