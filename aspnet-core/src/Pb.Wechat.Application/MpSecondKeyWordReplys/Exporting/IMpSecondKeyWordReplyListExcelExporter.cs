using Pb.Wechat.Dto;
using Pb.Wechat.MpSecondKeyWordReplys.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpSecondKeyWordReplys.Exporting
{
    public interface IMpSecondKeyWordReplyListExcelExporter
    {
        FileDto ExportToFile(List<MpSecondKeyWordReplyDto> modelListDtos);
    }
}
