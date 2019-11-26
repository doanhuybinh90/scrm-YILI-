using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaVoices.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpMediaVoices.Exporting
{
    public interface IMpMediaVoiceListExcelExporter
    {
        FileDto ExportToFile(List<MpMediaVoiceDto> modelListDtos);
    }
}
