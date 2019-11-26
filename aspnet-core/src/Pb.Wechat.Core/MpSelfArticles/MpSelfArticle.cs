using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;

namespace Pb.Wechat.MpSelfArticles
{
    public class MpSelfArticle : Entity<int>, IAudited, ISoftDelete
    {

        /// <summary>
        /// MpID
        /// </summary>
        [Description("MpID")]
        public int MpID { get; set; }

        /// <summary>
        /// MediaID
        /// </summary>
        [Description("PicFileID")]
        public string PicFileID { get; set; }
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
    }
}
