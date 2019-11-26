using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Pb.Wechat.MpMediaArticleGroups.Dto
{
    /// <summary>
    /// 只保留前台真正需要的字段，如果不需要则不吐出。
    /// </summary>
    [AutoMapFrom(typeof(MpMediaArticleGroupItem))]
    public class MpMediaArticleGroupItemDto : EntityDto<int>
    {
        public int MpID { get; set; } // 公众号ID 
        public int GroupID { get; set; } //多图文ID
        public int ArticleID { get; set; } // 单图文ID                                     
        public int? SortIndex { get; set; } // 排序
        public string Title { get; set; }//标题
    }
}
