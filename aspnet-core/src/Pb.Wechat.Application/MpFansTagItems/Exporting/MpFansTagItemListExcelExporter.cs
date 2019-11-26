using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpFansTagItems.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.MpFansTagItems.Exporting
{
    public class MpFansTagItemListExcelExporter : EpPlusExcelExporterBase, IMpFansTagItemListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpFansTagItemListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpFansTagItemDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    AddHeader(
                        sheet
,"CreationTime"
,"FansId"
,"IsDeleted"
,"MpID"
,"TagId"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.CreationTime
,_ => _.FansId
,_ => _.IsDeleted
,_ => _.MpID
,_ => _.TagId
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 5; i++)
                    {
                        if (i.IsIn(1, 5)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
