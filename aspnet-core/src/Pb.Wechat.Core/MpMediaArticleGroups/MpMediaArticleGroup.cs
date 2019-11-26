using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;

namespace Pb.Wechat.MpMediaArticleGroups
{
    public class MpMediaArticleGroup : Entity<int>, IAudited, ISoftDelete
    {

        /// <summary>
        /// MpID
        /// </summary>
        [Description("MpID")]
        public int MpID { get; set; }

        /// <summary>
        /// MediaID
        /// </summary>
        [Description("Name")]
        public string Name { get; set; }
        [Description("MediaID")]
        public string MediaID { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
