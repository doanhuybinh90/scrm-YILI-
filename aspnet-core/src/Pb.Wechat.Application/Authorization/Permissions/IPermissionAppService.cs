using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Authorization.Permissions.Dto;

namespace Pb.Wechat.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
