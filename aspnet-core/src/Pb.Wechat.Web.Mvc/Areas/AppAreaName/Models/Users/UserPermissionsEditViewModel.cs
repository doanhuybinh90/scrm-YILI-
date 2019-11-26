using Abp.AutoMapper;
using Pb.Wechat.Authorization.Users;
using Pb.Wechat.Authorization.Users.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.Common;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; private set; }

        public UserPermissionsEditViewModel(GetUserPermissionsForEditOutput output, User user)
        {
            User = user;
            output.MapTo(this);
        }
    }
}