﻿using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.CYConfigs.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CYConfigs.Exporting
{
    public class CYConfigListExcelExporter : EpPlusExcelExporterBase, ICYConfigListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public CYConfigListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<CYConfigDto> modelListDtos)
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
,"CloseProblemText"
,"CreateProblemText"
,"NoOperationMinute"
,"NoOperationText"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.CloseProblemText
,_ => _.CreateProblemText
,_ => _.NoOperationMinute
,_ => _.NoOperationText
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 4; i++)
                    {
                        if (i.IsIn(1, 4)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
