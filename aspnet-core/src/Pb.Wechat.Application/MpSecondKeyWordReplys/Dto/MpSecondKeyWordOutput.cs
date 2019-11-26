using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.MpSecondKeyWordReplys.Dto
{
    public class MpSecondKeyWordOutput : EntityDto<int>
    {
        public int ParentId { get; set; }
        public string KeyWord { get; set; }
        public string ReplyType { get; set; }
        public string ContentOrName { get; set; }
        public string FilePathOrUrl { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
