using Pb.Wechat.Dto;
using Pb.Wechat.MpFansTagItems.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpFansTagItems.Exporting
{
    public interface IMpFansTagItemListExcelExporter
    {
        FileDto ExportToFile(List<MpFansTagItemDto> modelListDtos);
    }
}
