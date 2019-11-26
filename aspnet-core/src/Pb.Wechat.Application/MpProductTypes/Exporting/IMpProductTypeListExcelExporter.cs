using Pb.Wechat.Dto;
using Pb.Wechat.MpProductTypes.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpProductTypes.Exporting
{
    public interface IMpProductTypeListExcelExporter
    {
        FileDto ExportToFile(List<MpProductTypeDto> modelListDtos);
    }
}
