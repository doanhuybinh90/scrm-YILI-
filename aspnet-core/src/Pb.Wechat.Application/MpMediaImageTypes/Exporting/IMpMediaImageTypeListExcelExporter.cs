using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaImageTypes.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpMediaImageTypes.Exporting
{
    public interface IMpMediaImageTypeListExcelExporter
    {
        FileDto ExportToFile(List<MpMediaImageTypeDto> modelListDtos);
    }
}
