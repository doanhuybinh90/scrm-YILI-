
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaVideos.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpMediaVideos.Exporting
{
    public interface IMpMediaVideoListExcelExporter
    {
        FileDto ExportToFile(List<MpMediaVideoDto> modelListDtos);
    }
}
