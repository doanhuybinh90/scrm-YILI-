using System.Collections.Generic;
using Pb.Wechat.Auditing.Dto;
using Pb.Wechat.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
