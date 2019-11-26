using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.MpMediaArticleGroups.Dto
{
    public class MediaArticleGroupOutput : EntityDto<int>
    {
        public DateTime? LastModificationTime { get; set; }
        public string MessageType { get; set; }
        public int? ArticleGroupID { get; set; }
        public int? ArticleID { get; set; }
        public string FilePathOrUrl { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public int RowSpan { get; set; }
        public string MediaID { get; set; }
        public int MpID { get; set; }
    }
}
