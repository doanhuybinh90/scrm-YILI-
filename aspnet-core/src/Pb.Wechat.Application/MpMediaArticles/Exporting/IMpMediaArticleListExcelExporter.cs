using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaArticles.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpMediaArticles.Exporting
{
    public interface IMpMediaArticleListExcelExporter
    {
        FileDto ExportToFile(List<MpMediaArticleDto> modelListDtos);
    }
}
