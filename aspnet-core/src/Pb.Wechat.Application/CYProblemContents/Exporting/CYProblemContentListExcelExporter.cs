using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.CYProblemContents.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CYProblemContents.Exporting
{
    public class CYProblemContentListExcelExporter : EpPlusExcelExporterBase, ICYProblemContentListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public CYProblemContentListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<CYProblemContentDto> modelListDtos)
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
,"Age"
,"CreationTime"
,"DoctorId"
,"File"
,"ProblemId"
,"SendUser"
,"Sex"
,"Text"
,"Type"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.Age
,_ => _.CreationTime
,_ => _.DoctorId
,_ => _.File
,_ => _.ProblemId
,_ => _.SendUser
,_ => _.Sex
,_ => _.Text
,_ => _.Type
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 9; i++)
                    {
                        if (i.IsIn(1, 9)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
