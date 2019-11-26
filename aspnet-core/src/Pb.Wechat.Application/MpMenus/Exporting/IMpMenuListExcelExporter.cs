
using Pb.Wechat.Dto;
using Pb.Wechat.MpMenus.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpMenus.Exporting
{
    public interface IMpMenuListExcelExporter
    {
        FileDto ExportToFile(List<MpMenuDto> modelListDtos);
    }
}
