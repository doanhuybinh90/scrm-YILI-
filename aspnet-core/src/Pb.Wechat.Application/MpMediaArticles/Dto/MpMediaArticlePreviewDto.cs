using Abp.Application.Services.Dto;
using System.ComponentModel;

namespace Pb.Wechat.MpMediaArticles.Dto
{
    //[AutoMap(typeof(MpMediaArticle))]
    public class MpMediaArticlePreviewDto : EntityDto<int>
    {

        [Description("MpID")]
        public int MpID { get; set; }


        [Description("MediaID")]

        public string MediaID { get; set; }

  
        [Description("NickName")]
 
        public string NickName { get; set; }
       
    }
}
