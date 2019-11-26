using System.Collections.Generic;
using System.Linq;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpChannels.Dto;
using Pb.Wechat.MpEvents.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public class MpChannelListExcelExporter : EpPlusExcelExporterBase, IMpChannelListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MpChannelListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<MpChannelDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    var typeEnum = EnumHelper.GetEnum(typeof(ChannelType));
                    var msgTypeEnum= EnumHelper.GetEnum(typeof(MpMessageType));
                    AddHeader(
                        sheet,
                        "名称",
                        "投放平台",
                        "二维码参数",

                         "渠道类型",
                        "渠道编码",
                        "回复类型", 
                        "二维码票据",
                        "二维码URL",
                        "二维码路径",

                        "开始时间",
                        "结束时间"
                        
                    );

                    AddObjects(
                        sheet, 2, modelListDtos,
                        _ => _.Name,
                       _=>_.PushActivityName,
                       _=>_.EventKey,
                       _ => typeEnum.Where(c => c.Item1 == _.ChannelType).FirstOrDefault()?.Item2,
                       _ => _.Code,
                       _ => msgTypeEnum.Where(c => c.Item1 == _.ReplyType).FirstOrDefault()?.Item2,
                       _ => _.Ticket,
                        _ => _.FileUrl,
                         _ => _.FilePath,
                       _ =>_.StartTime,
                       _=>_.EndTime
                        );

                    //Formatting cells

                    var timeColumn = sheet.Column(3);
                    timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    var timeColumnx = sheet.Column(4);
                    timeColumnx.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

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