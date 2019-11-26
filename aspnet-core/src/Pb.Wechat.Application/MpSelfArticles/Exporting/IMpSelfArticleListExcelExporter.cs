using Pb.Wechat.Dto;
using Pb.Wechat.MpSelfArticles.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpSelfArticles.Exporting
{
    public interface IMpSelfArticleListExcelExporter
    {
        FileDto ExportToFile(List<MpSelfArticleDto> modelListDtos);
    }
}
