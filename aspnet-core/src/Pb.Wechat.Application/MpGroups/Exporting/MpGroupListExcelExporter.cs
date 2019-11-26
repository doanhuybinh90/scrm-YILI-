using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpGroups.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpGroups.Exporting
{
    public class MpGroupListExcelExporter : EpPlusExcelExporterBase, IMpGroupListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpGroupListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpGroupDto> modelListDtos)
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
						,"ID"
,"MpID"
, "Name"
, "ParentID"
, "Length"
, "FullPath"
, "FansCount"
, "ChildCount"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
						,_ => _.Id
,_ => _.MpID
,_ => _.Name
,_ => _.ParentID
, _ => _.Length
, _ => _.FullPath
, _ => _.FansCount
, _ => _.ChildCount

                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(6);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 12; i++)
                    {
                        if (i.IsIn(1, 12)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
