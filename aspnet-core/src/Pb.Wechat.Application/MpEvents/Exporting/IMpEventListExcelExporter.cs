using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpEvents.Exporting
{
    public interface IMpEventListExcelExporter
    {
        FileDto ExportToFile(List<MpEventDto> modelListDtos);
        
    }
}
