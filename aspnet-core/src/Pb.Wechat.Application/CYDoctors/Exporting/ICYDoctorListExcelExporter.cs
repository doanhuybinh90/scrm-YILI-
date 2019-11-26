using Pb.Wechat.Dto;
using Pb.Wechat.CYDoctors.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CYDoctors.Exporting
{
    public interface ICYDoctorListExcelExporter
    {
        FileDto ExportToFile(List<CYDoctorDto> modelListDtos);
    }
}
