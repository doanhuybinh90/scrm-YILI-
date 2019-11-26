using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpChannels.Dto;
using Pb.Wechat.MpFans.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.MpFans.Exporting
{
    public class MpFanListExcelExporter : EpPlusExcelExporterBase, IMpFanListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpFanListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpFanDto> modelListDtos)
        {
            return CreateExcelPackage(
                "粉丝列表.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    var typeEnum = EnumHelper.GetEnum(typeof(ChannelEnum));
                    AddHeader(
                        sheet
						,"粉丝昵称"
,"OpenID"
, "关注状态"
, "关注时间"
, "关注渠道"
, "关注来源"
,"更新时间"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
						,_ => _.NickName
, _ => _.OpenID
,_ => _.IsFans?"已关注":"未关注"
,_ => _.SubscribeTime
, _ => typeEnum.Where(c => c.Item1 == _.ChannelType).FirstOrDefault()?.Item2
, _ => _.ChannelName
, _ => _.LastModificationTime

                        );

                    //Formatting cells

                    var timeColumn = sheet.Column(4);
                    timeColumn.Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                    var timeColumn2 = sheet.Column(7);
                    timeColumn2.Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

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
