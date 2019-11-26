using System.Collections.Generic;
using Pb.Wechat.Authorization.Permissions.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}