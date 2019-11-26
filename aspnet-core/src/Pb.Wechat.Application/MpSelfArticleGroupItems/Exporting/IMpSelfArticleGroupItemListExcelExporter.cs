
using Pb.Wechat.Dto;
using Pb.Wechat.MpSelfArticleGroupItems.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpSelfArticleGroupItems.Exporting
{
    public interface IMpSelfArticleGroupItemListExcelExporter
    {
        FileDto ExportToFile(List<MpSelfArticleGroupItemDto> modelListDtos);
    }
}
