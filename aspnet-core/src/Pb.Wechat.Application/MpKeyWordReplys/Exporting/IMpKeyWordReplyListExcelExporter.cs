using Pb.Wechat.Dto;
using Pb.Wechat.MpKeyWordReplys.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpKeyWordReplys.Exporting
{
    public interface IMpKeyWordReplyListExcelExporter
    {
        FileDto ExportToFile(List<MpKeyWordReplyDto> modelListDtos);
        
    }
}
