using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSolicitudeTexts.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.MpSolicitudeTexts.Exporting
{
    public class MpSolicitudeTextListExcelExporter : EpPlusExcelExporterBase, IMpSolicitudeTextListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpSolicitudeTextListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpSolicitudeTextDto> modelListDtos)
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
,"BabyAge"
,"BeginDay"
,"CreationTime"
,"CreatorUserId"
,"EndDay"
,"IsDeleted"
,"LastModificationTime"
,"LastModifierUserId"
,"MpID"
,"OneYearMonth"
,"OneYearWeek"
,"OverMonth"
,"OverYear"
,"SolicitudeText"
,"SolicitudeTextType"
,"UnbornWeek"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.BabyAge
,_ => _.BeginDay
,_ => _.CreationTime
,_ => _.CreatorUserId
,_ => _.EndDay
,_ => _.IsDeleted
,_ => _.LastModificationTime
,_ => _.LastModifierUserId
,_ => _.MpID
,_ => _.OneYearMonth
,_ => _.OneYearWeek
,_ => _.OverMonth
,_ => _.OverYear
,_ => _.SolicitudeText
,_ => _.SolicitudeTextType
,_ => _.UnbornWeek
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 16; i++)
                    {
                        if (i.IsIn(1, 17)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
