using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.MpEventRequestMsgLogs.Dto
{
    [AutoMap(typeof(MpEventRequestMsgLog))]
    public class MpEventRequestMsgLogDto : EntityDto<int>
    {
        public int MpID { get; set; }
        public string OpenID { get; set; }
        public string MsgType { get; set; }
        public string MsgId { get; set; }
        public string MediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AUrl { get; set; }
        public string Content { get; set; }
    }
}
