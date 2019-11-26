using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEventRequestMsgLogs.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpEventRequestMsgLogListExcelExporter
    {
        FileDto ExportToFile(List<MpEventRequestMsgLogDto> modelListDtos);
    }
}
