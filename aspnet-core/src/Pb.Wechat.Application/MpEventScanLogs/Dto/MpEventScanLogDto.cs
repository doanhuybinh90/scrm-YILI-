using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.MpEventScanLogs.Dto
{
    [AutoMap(typeof(MpEventScanLog))]
    public class MpEventScanLogDto : EntityDto<int>
    {
        public int MpID { get; set; }
        public string OpenID { get; set; }
        public string EventContent { get; set; }
        public string EventType { get; set; }
        public string MsgID { get; set; }
       
    }
}
