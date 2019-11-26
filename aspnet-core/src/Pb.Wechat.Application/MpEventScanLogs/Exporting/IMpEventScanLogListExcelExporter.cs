using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEventScanLogs.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpEventScanLogListExcelExporter
    {
        FileDto ExportToFile(List<MpEventScanLogDto> modelListDtos);
    }
}
