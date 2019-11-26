using System.Collections.Generic;
using Pb.Wechat.Authorization.Users.Dto;
using Pb.Wechat.Dto;

namespace Pb.Wechat.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}