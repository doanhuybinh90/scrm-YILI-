using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSecondKeyWordReplys.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpSecondKeyWordReplys.Exporting
{
    public class MpSecondKeyWordReplyListExcelExporter : EpPlusExcelExporterBase, IMpSecondKeyWordReplyListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpSecondKeyWordReplyListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpSecondKeyWordReplyDto> modelListDtos)
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
,"CreationTime"
,"CreatorUserId"
,"ImageID"
,"ImageMediaID"
,"ImageName"
,"IsDeleted"
,"KeyWord"
,"LastModificationTime"
,"LastModifierUserId"
,"MpID"
,"ParentId"
,"ReplyType"
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
,_ => _.CreationTime
,_ => _.CreatorUserId
,_ => _.ImageID
,_ => _.ImageMediaID
,_ => _.ImageName
,_ => _.IsDeleted
,_ => _.KeyWord
,_ => _.LastModificationTime
,_ => _.LastModifierUserId
,_ => _.MpID
,_ => _.ParentId
,_ => _.ReplyType
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

                    for (var i = 1; i <= 25; i++)
                    {
                        if (i.IsIn(1, 25)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
