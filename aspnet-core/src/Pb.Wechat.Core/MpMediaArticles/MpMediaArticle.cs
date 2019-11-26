using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpMediaArticles
{
    public class MpMediaArticle : Entity<int>, IAudited, ISoftDelete
    {

        [Required(ErrorMessage = "企业号ID不能为空")]
        [Description("MpID")]
        public int MpID { get; set; }

        
        [Description("MediaID")]
        [StringLength(200)]
        public string MediaID { get; set; }

        [Required(ErrorMessage = "企业号ID不能为空")]
        [Description("Title")]
        [StringLength(200)]
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [Description("Description")]
        public string Description { get; set; }
        /// <summary>
        /// FileID
        /// </summary>
        [Description("PicFileID")]
        [StringLength(500)]
        public string PicFileID { get; set; }
        [StringLength(200)]
        public string AUrl { get; set; }
        [Description("富文本内容")]
        public string Content { get; set; }
        [Description("上传到微信的富文本内容")]
        public string WxContent { get; set; }

        [StringLength(200)]
        public string PicMediaID { get; set; }
        [StringLength(1)]
        public string ShowPic { get; set; }
        [StringLength(500)]
        public string Author { get; set; }

        [StringLength(500)]
        public string FilePathOrUrl { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public string ArticleGrid { get; set; }
        public int EnableComment { get; set; }
        public int OnlyFansComment { get; set; }
    }
}
