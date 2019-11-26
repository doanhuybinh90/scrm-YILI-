using Pb.Wechat.Dto;
using Pb.Wechat.MpSolicitudeTexts.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpSolicitudeTexts.Exporting
{
    public interface IMpSolicitudeTextListExcelExporter
    {
        FileDto ExportToFile(List<MpSolicitudeTextDto> modelListDtos);
    }
}
