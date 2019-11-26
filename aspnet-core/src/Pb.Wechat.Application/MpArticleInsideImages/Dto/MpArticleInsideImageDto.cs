using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.MpArticleInsideImages.Dto
{
    [AutoMap(typeof(MpArticleInsideImage))]
    public class MpArticleInsideImageDto : EntityDto<int>
    {
        public int MpID { get; set; }
        public string LocalImageUrl { get; set; }
        public string WxImageUrl { get; set; }
        public string ArticleGrid { get; set; }
    }
}
