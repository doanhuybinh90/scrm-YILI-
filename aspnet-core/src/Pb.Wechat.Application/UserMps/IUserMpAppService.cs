using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.UserMps.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.UserMps
{
    public interface IUserMpAppService : IAsyncCrudAppService<UserMpDto, int, GetUserMpsInput, UserMpDto, UserMpDto>
    {
        Task<FileDto> GetListToExcel(GetUserMpsInput input);
        Task<int> GetDefaultMpId();
    }
}
