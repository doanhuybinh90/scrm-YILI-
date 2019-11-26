using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;

namespace Pb.Wechat.CustomerArticleGroupItems
{
    public class CustomerArticleGroupItem : Entity<int>, IAudited, ISoftDelete
    {
        // ID (Primary key)
                                       /// <summary>公众号ID</summary>	
        [Description("公众号ID")]
        public int MpID { get; set; } // MpID
                                         /// <summary>多图文ID</summary>	
        [Description("多图文ID")]
        public int GroupID { get; set; } // GroupID
                                            /// <summary>单图文ID</summary>	
        [Description("单图文ID")]
        public int ArticleID { get; set; } // ArticleID
                                              /// <summary>排序</summary>	
        [Description("排序")]
        public int? SortIndex { get; set; } // SortIndex
        [Description("单图文名称")]
        public string Title { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }

    
}
