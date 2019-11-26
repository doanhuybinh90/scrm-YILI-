using Pb.Wechat.MpEvents.Dto;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources.WxMediaHelper
{
    public interface IWxMediaUpload
    {
        Task<string> UploadAndGetMediaID(int mpid, string fileUrl, MpMessageType type);
        Task<string> UploadVideoAndGetMediaID(int mpid, string fileUrl, MpMessageType type,string title,string introduction);
        Task<string> UploadArtImageAndGetMediaID(int mpid, string fileUrl, MpMessageType type);
        Task<string> UploadArticleAndGetMediaID(int mpid, NewsModel newsModel,bool isCreate=true, string mediaID = null);
        Task UploadKfHeading(int mpid, string fileUrl,string kfAccount);
    }


}
