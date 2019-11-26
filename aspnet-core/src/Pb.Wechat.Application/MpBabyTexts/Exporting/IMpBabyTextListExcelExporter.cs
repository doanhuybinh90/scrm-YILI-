using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpBabyTexts.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpBabyTextListExcelExporter
    {
        FileDto ExportToFile(List<MpBabyTextDto> modelListDtos);
    }
}
