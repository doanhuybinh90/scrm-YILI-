using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpFansTags.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.MpFansTags.Exporting
{
    public class MpFansTagListExcelExporter : EpPlusExcelExporterBase, IMpFansTagListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpFansTagListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpFansTagDto> modelListDtos)
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
,"CreatorUserId"
,"FansCount"
,"IsDeleted"
,"LastModificationTime"
,"LastModifierUserId"
,"MpID"
,"Name"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.CreationTime
,_ => _.CreatorUserId
,_ => _.FansCount
,_ => _.IsDeleted
,_ => _.LastModificationTime
,_ => _.LastModifierUserId
,_ => _.MpID
,_ => _.Name
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 8; i++)
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
