using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpChannels.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpChannelListExcelExporter
    {
        FileDto ExportToFile(List<MpChannelDto> modelListDtos);
    }
}
