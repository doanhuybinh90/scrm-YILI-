using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Pb.Wechat.CustomerArticleGroupItems;
using System.ComponentModel;

namespace Pb.Wechat.CustomerArticleGroupItems.Dto
{
    [AutoMap(typeof(CustomerArticleGroupItem))]
    public class CustomerArticleGroupItemDto : EntityDto<int>
    {

        [Description("MpID")]
        public int MpID { get; set; }

        public int GroupID { get; set; }
        public int ArticleID { get; set; }
        public int? SortIndex { get; set; }
        public string Title { get; set; }

    }
}
