using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEventClickViewLogs.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpEventClickViewLogListExcelExporter
    {
        FileDto ExportToFile(List<MpEventClickViewLogDto> modelListDtos);
    }
}
