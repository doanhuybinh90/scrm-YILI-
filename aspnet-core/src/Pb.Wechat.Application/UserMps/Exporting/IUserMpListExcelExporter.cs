using Pb.Wechat.Dto;
using Pb.Wechat.UserMps.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.UserMps.Exporting
{
    public interface IUserMpListExcelExporter
    {
        FileDto ExportToFile(List<UserMpDto> modelListDtos);
    }
}
