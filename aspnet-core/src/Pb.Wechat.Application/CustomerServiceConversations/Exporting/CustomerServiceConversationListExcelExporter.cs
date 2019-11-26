using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceConversations.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.CustomerServiceConversations.Exporting
{
    public class CustomerServiceConversationListExcelExporter : EpPlusExcelExporterBase, ICustomerServiceConversationListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public CustomerServiceConversationListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<CustomerServiceConversationDto> modelListDtos)
        {
           
            return CreateExcelPackage(
                "会话明细表.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    var typeEnum = EnumHelper.GetEnum(typeof(CustomerServiceConversationState));
                    sheet.OutLineApplyStyle = true;
                    AddHeader(
                        sheet
,"创建时间"
,"粉丝OpenID"
,"开始会话时间"
, "结束会话时间"
, "会话状态"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.CreationTime
,_ => _.FanOpenId
,_ => _.StartTalkTime
, _ => _.EndTalkTime
, _ => _.State==0? "等待中":(_.State==1? "提问中":"结束")
                        );

                    //Formatting cells
                    var timeColumn1 = sheet.Column(1);
                    timeColumn1.Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
                    var timeColumn = sheet.Column(3);
                    timeColumn.Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
                    var timeColumn2 = sheet.Column(4);
                    timeColumn2.Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
                    for (var i = 1; i <= 13; i++)
                    {
                        if (i.IsIn(1, 13)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
