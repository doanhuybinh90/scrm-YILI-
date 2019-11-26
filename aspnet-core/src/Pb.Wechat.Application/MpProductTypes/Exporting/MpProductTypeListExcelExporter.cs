using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpProductTypes.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.MpProductTypes.Exporting
{
    public class MpProductTypeListExcelExporter : EpPlusExcelExporterBase, IMpProductTypeListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpProductTypeListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpProductTypeDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    AddHeader(
                        sheet
, "系列名称"
, "系列描述"
, "排序"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
, _ => _.Title
, _ => _.SubTitle
,_ => _.SortIndex
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 3; i++)
                    {
                        if (i.IsIn(1, 3)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
