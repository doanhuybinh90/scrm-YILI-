using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpMediaArticleGroups.Dto
{
    [AutoMapTo(typeof(MpMediaArticleGroup))]
    public class SaveMpMediaArticleGroupInput : EntityDto<int>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 多个图文素材的Ids
        /// </summary>
        [Required]
        public string ArticleIds { get; set; }

        public string MediaID { get; set; }
    }
}
