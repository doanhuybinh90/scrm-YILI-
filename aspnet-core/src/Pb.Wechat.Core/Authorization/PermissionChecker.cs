using Abp.Authorization;
using Pb.Wechat.Authorization.Roles;
using Pb.Wechat.Authorization.Users;

namespace Pb.Wechat.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
