using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.MpChannels.Dto
{
    public class MpChannelOutput : EntityDto<int>
    {
        public string Name { get; set; }
        public string ReplyType { get; set; }
        public string Code { get; set; }
        public string ChannelName { get; set; }
        public string EventKey { get; set; }
        public string ContentOrName { get; set; }
        public string FileUrl { get; set; }
        public string FilePath { get; set; }
        public string ChannelType { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int IsMcChannel { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string TagIds { get; set; }
        public string TagNames { get; set; }
    }
}
