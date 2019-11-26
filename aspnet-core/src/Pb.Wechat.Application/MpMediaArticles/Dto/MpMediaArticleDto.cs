using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.MpMediaArticles.Dto
{
    [AutoMap(typeof(MpMediaArticle))]
    public class MpMediaArticleDto : EntityDto<int>
    {

        [Description("MpID")]
        public int MpID { get; set; }


        [Description("MediaID")]

        public string MediaID { get; set; }

  
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
        [Description("PicFileID")]
 
        public string PicFileID { get; set; }

        public string AUrl { get; set; }
        public string Content { get; set; }
   
        public string PicMediaID { get; set; }
 
        public string ShowPic { get; set; }

        public string Author { get; set; }

        public string FilePathOrUrl { get; set; }

        public string HostName { get; set; }
        public string SavePath { get; set; }
        public DateTime? LastModificationTime { get; set; }
        [Description("上传到微信的富文本内容")]
        public string WxContent { get; set; }
        public string ArticleGrid { get; set; }
        public int EnableComment { get; set; }
        public int OnlyFansComment { get; set; }
    }
}
