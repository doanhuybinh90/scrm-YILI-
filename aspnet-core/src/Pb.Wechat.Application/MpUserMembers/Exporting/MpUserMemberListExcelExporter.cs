using System.Collections.Generic;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpUserMembers.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public class MpUserMemberListExcelExporter : EpPlusExcelExporterBase, IMpUserMemberListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MpUserMemberListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<MpUserMemberDto> modelListDtos)
        {
            return CreateExcelPackage(
                "会员信息.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    //var typeEnum = EnumHelper.GetEnum(typeof(MpUserMemeberType));
                    AddHeader(
                        sheet
                        , "会员名称"
, "生日"
, "手机"
, "OpenID"
, "关注来源"
, "绑定状态"
, "关注时间"
, "绑定时间"
, "注册时间"
, "解绑原因"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
                        , _ => _.MemberName
, _ => _.BabyBirthday
, _ => _.MobilePhone
, _ => _.OpenID
, _ => _.ChannelName
, _ => _.IsBinding?"已绑定":"未绑定"
, _ => _.SubscribeTime
, _ => _.BindingTime
, _ => _.RegisterTime
, _ => _.UnBindingReason
                        );


                    //Formatting cells

                    var timeColumn = sheet.Column(2);
                    timeColumn.Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";

                    var timeColumn1 = sheet.Column(7);
                    timeColumn1.Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
                    var timeColumn2 = sheet.Column(8);
                    timeColumn2.Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
                    var timeColumn3 = sheet.Column(9);
                    timeColumn3.Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
                    
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