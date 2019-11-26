using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel;

namespace Pb.Wechat.MpMenus.Dto
{
    [AutoMap(typeof(MpMenu))]
    public class MpMenuDto : EntityDto<int>
    {

        /// <summary>
        /// MpID
        /// </summary>
        [Description("MpID")]
        public int MpID { get; set; }
        public int ParentID { get; set; }
        /// <summary>
        /// MediaID
        /// </summary>
        [Description("FullPath")]
        public string FullPath { get; set; }
        public int? Length { get; set; }
        public int? ChildCount { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Description("Name")]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [Description("Type")]
        public string Type { get; set; }
        /// <summary>
        /// FileID
        /// </summary>
        [Description("MediaType")]
        public string MediaType { get; set; }
        [Description("GetOpenID")]
        public string GetOpenID { get; set; }
        public string LinkUrl { get; set; }
        public string Content { get; set; }
        public int ArticleID { get; set; }
        public string ArticleName { get; set; }
        public string ArticleMediaID { get; set; }
        public int ArticleGroupID { get; set; }
        public string ArticleGroupName { get; set; }
        public string ArticleGroupMediaID { get; set; }

        public int ImageID { get; set; }
        public string ImageName { get; set; }
        public string ImageMediaID { get; set; }
        public int VideoID { get; set; }
        public string VideoName { get; set; }
        public string VideoMediaID { get; set; }
        public int VoiceID { get; set; }
        public string VoiceName { get; set; }
        public string VoiceMediaID { get; set; }
        public int? SortIndex { get; set; }
        public string MenuKey { get; set; }
        public string MenuFullPath { get; set; }
        public bool UseSolicitude { get; set; }
    }
}
