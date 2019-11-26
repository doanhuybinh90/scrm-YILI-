using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpArticleInsideImages.Dto
{
    public class GetMpArticleInsideImagesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public string LocalImageUrl { get; set; }
        public string WxImageUrl { get; set; }
        public string ArticleGrid { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
