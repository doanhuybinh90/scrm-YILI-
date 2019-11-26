using Pb.Wechat.Dto;
using Pb.Wechat.MpMessages.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpMessages.Exporting
{
    public interface IMpMessageListExcelExporter
    {
        FileDto ExportToFile(List<MpMessageDto> modelListDtos);
        
    }
}
