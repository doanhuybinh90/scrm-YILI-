using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpMediaArticles.Dto
{
    public class GetMpMediaArticlesInput : PagedAndSortedInputDto, IShouldNormalize
    {

        public int MpID { get; set; }


        public string MediaID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PicFileID { get; set; }

        public string AUrl { get; set; }
        public string Content { get; set; }

        public string PicMediaID { get; set; }

        public string ShowPic { get; set; }
  
        public string Author { get; set; }

        public string FilePathOrUrl { get; set; }

        public string WxContent { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
            
        }
    }
}
