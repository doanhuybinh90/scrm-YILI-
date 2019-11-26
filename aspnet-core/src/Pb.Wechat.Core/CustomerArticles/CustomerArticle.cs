using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.CustomerArticles
{
    public class CustomerArticle : Entity<int>, IAudited, ISoftDelete
    {

        /// <summary>
        /// MpID
        /// </summary>
        [Description("MpID")]
        public int MpID { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Description("Title")]
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [Description("Description")]
        public string Description { get; set; }
        /// <summary>
        /// FileID
        /// </summary>
        [Description("AUrl")]
        public string AUrl { get; set; }
        [Description("PicMediaID")]
        public string PicMediaID { get; set; }
        public string FilePathOrUrl { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int TypeId { get; set; }
        [StringLength(100)]
        public string TypeName { get; set; }
        [StringLength(100)]
        public string MediaID { get; set; }
    }
}
