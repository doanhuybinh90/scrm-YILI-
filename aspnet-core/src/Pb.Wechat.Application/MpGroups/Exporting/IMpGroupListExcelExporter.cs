using Pb.Wechat.Dto;
using Pb.Wechat.MpGroups.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpGroups.Exporting
{
    public interface IMpGroupListExcelExporter
    {
        FileDto ExportToFile(List<MpGroupDto> modelListDtos);
    }
}
