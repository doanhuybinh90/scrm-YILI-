using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpProductInfos.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpProductInfoListExcelExporter
    {
        FileDto ExportToFile(List<MpProductInfoDto> modelListDtos);
    }
}
