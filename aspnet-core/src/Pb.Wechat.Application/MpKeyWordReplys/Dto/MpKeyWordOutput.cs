using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.MpKeyWordReplys.Dto
{
    public class MpKeyWordOutput : EntityDto<int>
    {
         public string KeyWord { get; set; }
        public string ReplyType { get; set; }
        public string ContentOrName { get; set; }
        public string FilePathOrUrl { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
