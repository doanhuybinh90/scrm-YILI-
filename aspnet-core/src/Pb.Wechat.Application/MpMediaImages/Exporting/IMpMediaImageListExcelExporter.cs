using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaImages.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpMediaImages.Exporting
{
    public interface IMpMediaImageListExcelExporter
    {
        FileDto ExportToFile(List<MpMediaImageDto> modelListDtos);
    }
}
