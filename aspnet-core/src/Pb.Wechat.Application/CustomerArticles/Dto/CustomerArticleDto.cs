﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.CustomerArticles.Dto
{
    [AutoMap(typeof(CustomerArticle))]
    public class CustomerArticleDto : EntityDto<int>
    {
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
        /// PicFileID
        /// </summary>
        [Description("PicFileID")]
        public string PicFileID { get; set; }
/// <summary>
        /// Url
        /// </summary>
        [Description("AUrl")]
        public string AUrl { get; set; }
        public string PicMediaID { get; set; }
        public string FilePathOrUrl { get; set; }
        public DateTime LastModificationTime { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string MediaID { get; set; }
    }
}
