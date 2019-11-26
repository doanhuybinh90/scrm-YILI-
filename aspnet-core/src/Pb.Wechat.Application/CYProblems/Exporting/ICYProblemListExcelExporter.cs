using Pb.Wechat.Dto;
using Pb.Wechat.CYProblems.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CYProblems.Exporting
{
    public interface ICYProblemListExcelExporter
    {
        FileDto ExportToFile(List<CYProblemDto> modelListDtos);
    }
}
