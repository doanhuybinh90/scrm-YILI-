using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.CYDoctors.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CYDoctors.Exporting
{
    public class CYDoctorListExcelExporter : EpPlusExcelExporterBase, ICYDoctorListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public CYDoctorListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<CYDoctorDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    //var typeEnum = EnumHelper.GetEnum(typeof(RewardTypes));
                    AddHeader(
                        sheet
,"CYId"
,"Clinic"
,"ClinicNO"
,"CreationTime"
,"GoodAt"
,"Hospital"
,"HospitalGrand"
,"Image"
,"LevelTitle"
,"Name"
,"Title"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.CYId
,_ => _.Clinic
,_ => _.ClinicNO
,_ => _.CreationTime
,_ => _.GoodAt
,_ => _.Hospital
,_ => _.HospitalGrand
,_ => _.Image
,_ => _.LevelTitle
,_ => _.Name
,_ => _.Title
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 11; i++)
                    {
                        if (i.IsIn(1, 11)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
