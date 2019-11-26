using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel;

namespace Pb.Wechat.MpSelfArticleGroupItems.Dto
{
    [AutoMap(typeof(MpSelfArticleGroupItem))]
    public class MpSelfArticleGroupItemDto : EntityDto<int>
    {

        [Description("MpID")]
        public int MpID { get; set; }

        public int GroupID { get; set; }
        public int ArticleID { get; set; }
        public int? SortIndex { get; set; }
        public string Title { get; set; }

    }
}
