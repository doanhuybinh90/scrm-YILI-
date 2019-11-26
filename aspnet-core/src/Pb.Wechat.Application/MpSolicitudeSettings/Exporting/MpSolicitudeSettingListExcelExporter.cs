using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSolicitudeSettings.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.MpSolicitudeSettings.Exporting
{
    public class MpSolicitudeSettingListExcelExporter : EpPlusExcelExporterBase, IMpSolicitudeSettingListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpSolicitudeSettingListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpSolicitudeSettingDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    AddHeader(
                        sheet
,"ArticleGroupID"
,"ArticleGroupMediaID"
,"ArticleGroupName"
,"ArticleID"
,"ArticleMediaID"
,"ArticleName"
,"Content"
,"DelayMinutes"
,"ImageID"
,"ImageMediaID"
,"ImageName"
,"MediaType"
,"MpID"
,"VideoID"
,"VideoMediaID"
,"VideoName"
,"VoiceID"
,"VoiceMediaID"
,"VoiceName"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.ArticleGroupID
,_ => _.ArticleGroupMediaID
,_ => _.ArticleGroupName
,_ => _.ArticleID
,_ => _.ArticleMediaID
,_ => _.ArticleName
,_ => _.Content
,_ => _.DelayMinutes
,_ => _.ImageID
,_ => _.ImageMediaID
,_ => _.ImageName
,_ => _.MediaType
,_ => _.MpID
,_ => _.VideoID
,_ => _.VideoMediaID
,_ => _.VideoName
,_ => _.VoiceID
,_ => _.VoiceMediaID
,_ => _.VoiceName
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 19; i++)
                    {
                        if (i.IsIn(1, 19)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
