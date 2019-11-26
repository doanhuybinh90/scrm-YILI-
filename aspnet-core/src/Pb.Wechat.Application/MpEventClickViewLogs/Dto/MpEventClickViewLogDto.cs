using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.MpEventClickViewLogs.Dto
{
    [AutoMap(typeof(MpEventClickViewLog))]
    public class MpEventClickViewLogDto : EntityDto<int>
    {
        public int MpID { get; set; }
        public string OpenID { get; set; }
        public string EventKey { get; set; }
        public string EventType { get; set; }
        public string MsgID { get; set; }
       
    }
}
