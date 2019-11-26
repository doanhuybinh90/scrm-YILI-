using System.Collections.Generic;
using Pb.Wechat.Authorization.Users.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}