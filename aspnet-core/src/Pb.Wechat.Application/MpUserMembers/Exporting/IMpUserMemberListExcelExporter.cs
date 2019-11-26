using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpUserMembers.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpUserMemberListExcelExporter
    {
        FileDto ExportToFile(List<MpUserMemberDto> modelListDtos);
    }
}
