using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CYConfigs.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.CYConfigs
{
    public interface ICYConfigAppService : IAsyncCrudAppService<CYConfigDto, int, GetCYConfigsInput, CYConfigDto, CYConfigDto>
    {
        Task<FileDto> GetListToExcel(GetCYConfigsInput input);

        Task<CYConfigDto> GetConfig();
    }
}
