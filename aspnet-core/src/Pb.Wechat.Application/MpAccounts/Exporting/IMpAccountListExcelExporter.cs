using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpAccounts.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpAccountListExcelExporter
    {
        FileDto ExportToFile(List<MpAccountDto> modelListDtos);
    }
}
