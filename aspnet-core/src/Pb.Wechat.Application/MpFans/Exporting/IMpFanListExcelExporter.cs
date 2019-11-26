using Pb.Wechat.Dto;
using Pb.Wechat.MpFans.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpFans.Exporting
{
    public interface IMpFanListExcelExporter
    {
        FileDto ExportToFile(List<MpFanDto> modelListDtos);
    }
}
