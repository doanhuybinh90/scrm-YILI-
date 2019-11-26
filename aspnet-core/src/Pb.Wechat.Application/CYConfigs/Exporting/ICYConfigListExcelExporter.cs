using Pb.Wechat.Dto;
using Pb.Wechat.CYConfigs.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CYConfigs.Exporting
{
    public interface ICYConfigListExcelExporter
    {
        FileDto ExportToFile(List<CYConfigDto> modelListDtos);
    }
}
