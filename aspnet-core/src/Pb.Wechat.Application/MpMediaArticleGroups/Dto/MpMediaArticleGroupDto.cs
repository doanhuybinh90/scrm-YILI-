using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Pb.Wechat.MpMediaArticleGroups.Dto
{
    [AutoMap(typeof(MpMediaArticleGroup))]
    public class MpMediaArticleGroupDto : EntityDto<int>
    {

        /// <summary>
        /// MpID
        /// </summary>
        [Description("MpID")]
        public int MpID { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Description("Name")]
        public string Name { get; set; }

        public string MediaID { get; set; }
        /// <summary>
        /// 多个图文素材的Ids
        /// </summary>
        public string ArticleIds { get; set; }
        public virtual ICollection<MpMediaArticleGroupItem> MpMediaArticleGroupItem { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
