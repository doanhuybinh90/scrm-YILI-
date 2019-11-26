using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.MpEvents.Exporting
{
    public class MpEventListExcelExporter : EpPlusExcelExporterBase, IMpEventListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MpEventListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<MpEventDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    var typeEnum = EnumHelper.GetEnum(typeof(MpEventType));
                    var msgTypeEnum = EnumHelper.GetEnum(typeof(MpMessageType));
                    AddHeader(
                        sheet,
                        "企业号ID",
                        "事件类型",
                        "事件编号",
                        "回复类型",
                        "回复内容",
                        "素材记录ID",
                        "素材记录名称",
                        "微信素材ID",
                        "素材记录ID",
                        "素材记录名称",
                        "微信素材ID",
                        "素材记录ID",
                        "素材记录名称",
                        "微信素材ID",
                        "素材记录ID",
                        "素材记录名称",
                        "微信素材ID",
                        "素材记录ID",
                        "素材记录名称",
                        "微信素材ID"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos,
                        _ => _.MpID,
                        _ => typeEnum.Where(c => c.Item1 == _.EventType).FirstOrDefault()?.Item2,
                        _ => _.EventCode,
                        _ => msgTypeEnum.Where(c => c.Item1 == _.ReplyType).FirstOrDefault()?.Item2,
                        _ => _.Content,
                        _ => _.ArticleID,
                        _ => _.ArticleName,
                        _ => _.ArticleMediaID,
                        _ => _.ArticleGroupID,
                        _ => _.ArticleGroupName,
                        _ => _.ArticleGroupMediaID,
                        _ => _.ImageID,
                        _ => _.ImageName,
                        _=>_.ImageMediaID,
                         _ => _.VideoID,
                        _ => _.VideoName,
                        _ => _.VideoMediaID,
                         _ => _.VoiceID,
                        _ => _.VoiceName,
                        _ => _.VoiceMediaID
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(12);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 10; i++)
                    {
                        if (i.IsIn(5, 10)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }

       
    }
}
