using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpCourseSignups.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpCourseSignupListExcelExporter
    {
        FileDto ExportToFile(List<MpCourseSignupDto> modelListDtos);
    }
}
