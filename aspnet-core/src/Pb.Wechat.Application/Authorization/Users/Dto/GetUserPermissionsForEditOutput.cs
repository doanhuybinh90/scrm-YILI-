using System.Collections.Generic;
using Pb.Wechat.Authorization.Permissions.Dto;

namespace Pb.Wechat.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}