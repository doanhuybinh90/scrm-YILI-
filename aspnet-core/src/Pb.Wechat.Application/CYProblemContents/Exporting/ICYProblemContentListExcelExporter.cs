using Pb.Wechat.Dto;
using Pb.Wechat.CYProblemContents.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CYProblemContents.Exporting
{
    public interface ICYProblemContentListExcelExporter
    {
        FileDto ExportToFile(List<CYProblemContentDto> modelListDtos);
    }
}
