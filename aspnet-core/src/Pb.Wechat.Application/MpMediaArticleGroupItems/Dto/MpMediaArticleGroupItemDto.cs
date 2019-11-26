using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaArticleGroupItems.Dto
{
    [AutoMap(typeof(MpMediaArticleGroupItem))]
    public class MpMediaArticleGroupItemDto : EntityDto<int>
    {
        
        public int MpID { get; set; } // 公众号ID 
        public int GroupID { get; set; } //多图文ID
        public int ArticleID { get; set; } // 单图文ID                                     
        public int? SortIndex { get; set; } // 排序

        public string Title { get; set; }//标题

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
