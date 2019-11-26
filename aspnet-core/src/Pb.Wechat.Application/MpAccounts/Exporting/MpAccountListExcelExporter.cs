using System.Collections.Generic;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpAccounts.Dto;
using System.Linq;

namespace Pb.Wechat.Auditing.Exporting
{
    public class MpAccountListExcelExporter : EpPlusExcelExporterBase, IMpAccountListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MpAccountListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<MpAccountDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    var typeEnum = EnumHelper.GetEnum(typeof(MpAccountType));
                    AddHeader(
                        sheet,
                        "名称",
                        "类型",
                        "AppId",
                        "AppSecret",
                        "Token",
                        "EncodingAESKey",
                        "商户号",
                        "支付AppSecret",
                        "证书路径",
                        "证书密码",
                        "备注"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos,
                        _ => _.Name,
                        _ => typeEnum.Where(c => c.Item1 == _.AccountType).FirstOrDefault()?.Item2,
                        _ => _.AppId,
                        _ => _.AppSecret,
                        _ => _.Token,
                        _ => _.EncodingAESKey,
                        _ => _.MchID,
                        _ => _.WxPayAppSecret,
                        _ => _.CertPhysicalPath,
                        _ => _.CertPassword,
                        _ => _.Remark
                        );

                    //Formatting cells

                    var timeColumn = sheet.Column(12);
                    timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

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