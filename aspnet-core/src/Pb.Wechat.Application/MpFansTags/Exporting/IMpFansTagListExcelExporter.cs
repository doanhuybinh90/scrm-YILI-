using Pb.Wechat.Dto;
using Pb.Wechat.MpFansTags.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpFansTags.Exporting
{
    public interface IMpFansTagListExcelExporter
    {
        FileDto ExportToFile(List<MpFansTagDto> modelListDtos);
    }
}
