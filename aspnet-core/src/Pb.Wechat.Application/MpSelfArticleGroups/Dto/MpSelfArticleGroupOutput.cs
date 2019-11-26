using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.MpSelfArticleGroups.Dto
{
    public class MpSelfArticleGroupOutput : EntityDto<int>
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
        public string AUrl { get; set; }
        public string Description { get; set; }
    }
}
