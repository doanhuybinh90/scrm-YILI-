using Pb.Wechat.Dto;
using Pb.Wechat.MpSolicitudeSettings.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpSolicitudeSettings.Exporting
{
    public interface IMpSolicitudeSettingListExcelExporter
    {
        FileDto ExportToFile(List<MpSolicitudeSettingDto> modelListDtos);
    }
}
