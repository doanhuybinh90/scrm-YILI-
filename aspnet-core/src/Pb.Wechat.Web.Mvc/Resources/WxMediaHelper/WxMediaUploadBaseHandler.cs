using Abp.UI;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.Web.Helpers;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources.WxMediaHelper
{
    public class WxMediaUploadBaseHandler : IWxMediaUpload
    {
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly ILogger Logger;
        public WxMediaUploadBaseHandler(IAccessTokenContainer accessTokenContainer, IMpAccountAppService mpAccountAppService,ILogger _Logger)
        {
            _accessTokenContainer = accessTokenContainer;
            _mpAccountAppService = mpAccountAppService;
            Logger = _Logger;
        }
        public virtual async Task<string> UploadAndGetMediaID(int mpid, string fileUrl, MpMessageType type)
        {
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            UploadForeverMediaResult responseModel = null;
            var fileName = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
            try
            {

                responseModel = JsonConvert.DeserializeObject<UploadForeverMediaResult>(await HttpHelper.HttpPost($"https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={access_token}&type={type.ToString()}", fileUrl, fileName));
            }
            catch(Exception ex)
            {
                Logger.Error($"上传图片到微信出错：{ex.Message}");
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    responseModel = JsonConvert.DeserializeObject<UploadForeverMediaResult>(await HttpHelper.HttpPost($"https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={access_token}&type={type.ToString()}", fileUrl, fileName));
                }
                catch (Exception e)
                {
                    Logger.Error($"上传图片到微信出错：{e.Message}");
                    throw new UserFriendlyException(e.Message);
                }

            }
            return responseModel != null ? responseModel.media_id : "";
        }

        public virtual async Task<string> UploadVideoAndGetMediaID(int mpid, string fileUrl, MpMessageType type, string title, string introduction)
        {
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            UploadForeverMediaResult responseModel = null;
            var fileName = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
            Dictionary<string, string> content = new Dictionary<string, string>() { { "description", JsonConvert.SerializeObject(new { title = title, introduction = introduction }) } };


            try
            {

                responseModel = JsonConvert.DeserializeObject<UploadForeverMediaResult>(await HttpHelper.HttpPostVideo($"https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={access_token}&type={type.ToString()}", fileUrl, content.First(), fileName));
            }
            catch
            {

                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    responseModel = JsonConvert.DeserializeObject<UploadForeverMediaResult>(await HttpHelper.HttpPostVideo($"https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={access_token}&type={type.ToString()}", fileUrl, content.First(), fileName));
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException(e.Message);
                }

            }
            return responseModel != null ? responseModel.media_id : "";
        }

        public virtual async Task<string> UploadArtImageAndGetMediaID(int mpid, string fileUrl, MpMessageType type)
        {
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            UploadImgResult responseModel = null;
            var fileName = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
            try
            {

                responseModel = JsonConvert.DeserializeObject<UploadImgResult>(await HttpHelper.HttpPost($"https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={access_token}&type={type.ToString()}", fileUrl, fileName));
            }
            catch
            {

                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    responseModel = JsonConvert.DeserializeObject<UploadImgResult>(await HttpHelper.HttpPost($"https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={access_token}&type={type.ToString()}", fileUrl, fileName));
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException(e.Message);
                }

            }
            return responseModel != null ? responseModel.url : "";
        }
        public virtual async Task<string> UploadArticleAndGetMediaID(int mpid, NewsModel newsModel, bool isCreate = true, string mediaID = null)
        {
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);

            try
            {
                if (isCreate)
                    return (await MediaApi.UploadNewsAsync(access_token, Senparc.Weixin.Config.TIME_OUT, newsModel)).media_id;
                else
                {
                    await MediaApi.UpdateForeverNewsAsync(access_token, mediaID, 0, newsModel, Senparc.Weixin.Config.TIME_OUT);
                    return mediaID;
                }

            }
            catch
            {

                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    if (isCreate)
                        return (await MediaApi.UploadNewsAsync(access_token, Senparc.Weixin.Config.TIME_OUT, newsModel)).media_id;
                    else
                    {
                        await MediaApi.UpdateForeverNewsAsync(access_token, mediaID, 0, newsModel, Senparc.Weixin.Config.TIME_OUT);
                        return mediaID;
                    }

                }
                catch (Exception e)
                {
                    throw new UserFriendlyException(e.Message);
                }
            }

        }

        public virtual async Task UploadKfHeading(int mpid, string fileUrl, string kfAccount)
        {
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            WxJsonResult responseModel = null;
            var fileName = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
            try
            {
                Logger.Info($"客服头像文件名称{fileName}");
                responseModel = JsonConvert.DeserializeObject<WxJsonResult>(await HttpHelper.HttpPost($"https://api.weixin.qq.com/customservice/kfaccount/uploadheadimg?access_token={access_token}&kf_account={kfAccount}", fileUrl, fileName));
            }
            catch
            {
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    responseModel = JsonConvert.DeserializeObject<WxJsonResult>(await HttpHelper.HttpPost($"https://api.weixin.qq.com/customservice/kfaccount/uploadheadimg?access_token={access_token}&kf_account={kfAccount}", fileUrl, fileName));
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException(e.Message);
                }

            }
        }
    }


}
