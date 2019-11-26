using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.CYProblems.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CYProblems.Exporting
{
    public class CYProblemListExcelExporter : EpPlusExcelExporterBase, ICYProblemListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public CYProblemListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<CYProblemDto> modelListDtos)
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
,"CYProblemId"
,"CreationTime"
,"FansId"
,"OpenId"
,"State"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.CYProblemId
,_ => _.CreationTime
,_ => _.FansId
,_ => _.OpenId
,_ => _.State
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 5; i++)
                    {
                        if (i.IsIn(1, 8)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
