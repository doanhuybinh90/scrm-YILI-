using Abp.Dependency;
using Pb.Wechat.MpMenus;
using Pb.Wechat.WxMedias.Dto;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs.CustomService;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.WxMedias
{
    public interface IWxMediaAppService : ITransientDependency
    {
        Task<string> UploadMedia(string fileFullPath, string mediaID);
        Task DelFileFromWx(int mpid, string mediaID);
        Task<string> UploadVideo(string fileFullPath, string mediaID, string title, string describe);
        Task<WxJsonResult> SyncMenu(List<MpMenu> menus);
        Task SetPreviewCache(string messageType, string mediaID, string openId);
        Task<PreviewModel> GetPreviewCache(string userId);

        Task<SendResult> PreviewMatial(PreviewModel model);

        Task<string> UploadArticleInImage(int mpid,string filefullPath);

        //Task<CustomInfoJson> GetCurstomerList(int mpId);
        Task<CustomListInfo> GetCurstomerList(int mpId);
        
        Task<WxJsonResult> AddCustom(int mpId, string kfAccount, string nickName, string passWord, string inviteWx);
        Task<WxJsonResult> UpdateCustom(int mpId, string kfAccount, string nickName, string passWord);
        Task<WxJsonResult> UploadCustomHeadimg(int mpId, string kfAccount, string filePath);
        Task<WxJsonResult> DeleteCustom(int mpId, string kfAccount);
        Task<QrCodeResult> SaveQrCode(int mpid, string name, string eventKey, string channelType, int expireSeconds = 0);


    }
}
