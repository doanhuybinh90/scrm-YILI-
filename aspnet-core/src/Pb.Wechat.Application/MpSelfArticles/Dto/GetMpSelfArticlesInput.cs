using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpSelfArticles.Dto
{
    public class GetMpSelfArticlesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string PicFileID { get; set; }

        public string AUrl { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id DESC";
            }
        }
    }
}
