
using Pb.Wechat.Dto;
using Pb.Wechat.MpSelfArticleGroups.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpSelfArticleGroups.Exporting
{
    public interface IMpSelfArticleGroupListExcelExporter
    {
        FileDto ExportToFile(List<MpSelfArticleGroupDto> modelListDtos);
    }
}
