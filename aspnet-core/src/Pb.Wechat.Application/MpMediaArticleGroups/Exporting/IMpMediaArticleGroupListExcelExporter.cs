
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaArticleGroups.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpMediaArticleGroups.Exporting
{
    public interface IMpMediaArticleGroupListExcelExporter
    {
        FileDto ExportToFile(List<MpMediaArticleGroupDto> modelListDtos);
    }
}
