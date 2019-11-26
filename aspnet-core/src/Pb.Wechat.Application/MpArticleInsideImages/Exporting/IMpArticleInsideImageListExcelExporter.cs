using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpArticleInsideImages.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpArticleInsideImageListExcelExporter
    {
        FileDto ExportToFile(List<MpArticleInsideImageDto> modelListDtos);
    }
}
