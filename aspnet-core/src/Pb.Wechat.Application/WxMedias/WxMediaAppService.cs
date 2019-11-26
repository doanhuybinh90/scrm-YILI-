
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpMenus;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias.Dto;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.CustomService;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace Pb.Wechat.WxMedias
{
    public class WxMediaAppService : IWxMediaAppService
    {
        private readonly ILogger Logger;
        private readonly IAppFolders _appFolders;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly IUserMpAppService _userMpAppService;
        private readonly ICacheManager _cacheManager;
        private readonly IAbpSession _abpSession;
        private readonly IHostingEnvironment _env;
        public WxMediaAppService(IUserMpAppService userMpAppService, IAppFolders appFolders, IMpAccountAppService mpAccountAppService, IAccessTokenContainer accessTokenContainer, ILogger _logger, ICacheManager cacheManager, IAbpSession abpSession, IHostingEnvironment env)
        {
            _appFolders = appFolders;
            _mpAccountAppService = mpAccountAppService;
            _accessTokenContainer = accessTokenContainer;
            _userMpAppService = userMpAppService;
            Logger = _logger;
            _cacheManager = cacheManager;
            _abpSession = abpSession;
            _env = env;
        }

        #region 素材
        public async Task<string> UploadVideo(string fileFullPath, string mediaID, string title, string describe)
        {
            if (string.IsNullOrWhiteSpace(fileFullPath))
                return mediaID;
            var tempProfilePicturePath = Path.Combine(_appFolders.TempFileDownloadFolder, fileFullPath);
            var mpid = await _userMpAppService.GetDefaultMpId();
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);

            var result = "";

            try
            {
                var postResult = Pb.Wechat.WxMedias.UploadVideo.GetUploadVideoResult(access_token, tempProfilePicturePath, title, describe);
                var postResultModel = JsonConvert.DeserializeObject<PostResult>(postResult);
                result = postResultModel.media_id;
                //result = MediaApi.UploadForeverVideo(access_token, tempProfilePicturePath, title, describe).media_id;

            }
            catch
            {

                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    var postResult = Pb.Wechat.WxMedias.UploadVideo.GetUploadVideoResult(access_token, tempProfilePicturePath, title, describe);
                    var postResultModel = JsonConvert.DeserializeObject<PostResult>(postResult);
                    result = postResultModel.media_id;

                }
                catch (Exception e)
                {

                    throw new UserFriendlyException(e.Message);
                }

            }

            return result;
        }
        public async Task DelFileFromWx(int mpid, string mediaID)
        {
            var account = _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid }).Result;
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);

            try
            {
                await MediaApi.DeleteForeverMediaAsync(access_token, mediaID);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

        }
        public async Task<string> UploadMedia(string fileFullPath, string mediaID)
        {

            if (string.IsNullOrWhiteSpace(fileFullPath))
                return mediaID;
            var tempProfilePicturePath = Path.Combine(_appFolders.TempFileDownloadFolder, fileFullPath);
            var mpid = await _userMpAppService.GetDefaultMpId();
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid });

            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);


            Senparc.Weixin.MP.AdvancedAPIs.Media.UploadForeverMediaResult responseModel = null;


            var result = "";
            try
            {

                responseModel = MediaApi.UploadForeverMedia(access_token, tempProfilePicturePath);

                //result = MediaApi.UploadForeverMedia(access_token, tempProfilePicturePath).media_id;
                result = responseModel.media_id;

            }
            catch
            {

                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    responseModel = MediaApi.UploadForeverMedia(access_token, tempProfilePicturePath);
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException(e.Message);
                }


                //result = MediaApi.UploadForeverMedia(access_token, tempProfilePicturePath).media_id;
                result = responseModel.media_id;

            }
            return result;
        }
        public async Task<string> UploadArticleInImage(int mpid, string filefullPath)
        {
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid });
            var tempProfilePicturePath = Path.Combine(_env.WebRootPath, filefullPath);
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            Senparc.Weixin.MP.AdvancedAPIs.Media.UploadImgResult responseModel = null;
            var result = "";
            try
            {
                responseModel = await MediaApi.UploadImgAsync(access_token, tempProfilePicturePath);
                result = responseModel.url;

            }
            catch
            {
                try
                {
                    access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                    responseModel = await MediaApi.UploadImgAsync(access_token, tempProfilePicturePath);
                    result = responseModel.url;
                }
                catch (Exception e)
                {

                    throw new UserFriendlyException(e.Message);
                }


            }
            return result;
        }
        #endregion

        public async Task SetPreviewCache(string messageType, string mediaID, string openId)
        {
            var currId = _abpSession.UserId;
            await _cacheManager.GetCache(AppConsts.Cache_PreviewMartial).SetAsync($"{currId}", new PreviewModel
            {
                MediaID = mediaID,
                MessageType = messageType,
                OpenID = openId
            });
        }

        public async Task<PreviewModel> GetPreviewCache(string userId)
        {
            return await _cacheManager.GetCache(AppConsts.Cache_PreviewMartial).GetOrDefaultAsync<string, PreviewModel>($"{userId}");
        }

        #region 菜单
        public async Task<WxJsonResult> SyncMenu(List<MpMenu> menus)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var account = _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid }).Result;
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            var bg = new ButtonGroup();
            foreach (var rootButton in menus.Where(c => c.Length == 1))
            {
                var subbuttons = menus.Where(c => c.ParentID == rootButton.Id);
                #region 无子菜单
                if (subbuttons.Count() == 0)
                {
                    //底部单击按钮
                    if (rootButton.Type == null ||
                        (rootButton.Type.Equals("CLICK", StringComparison.OrdinalIgnoreCase) && rootButton.Id == 0))
                    {
                        throw new WeixinMenuException("单击按钮的key不能为空！");
                    }

                    if (rootButton.Type.Equals("CLICK", StringComparison.OrdinalIgnoreCase))
                    {
                        //点击
                        bg.button.Add(new SingleClickButton()
                        {
                            name = rootButton.Name,
                            key = rootButton.MenuKey,
                            type = rootButton.Type
                        });
                    }
                    else if (rootButton.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase))
                    {
                        //URL
                        bg.button.Add(new SingleViewButton()
                        {
                            name = rootButton.Name,
                            url = rootButton.LinkUrl,
                            type = rootButton.Type
                        });
                    }
                    else if (rootButton.Type.Equals("LOCATION_SELECT", StringComparison.OrdinalIgnoreCase))
                    {
                        //弹出地理位置选择器
                        bg.button.Add(new SingleLocationSelectButton()
                        {
                            name = rootButton.Name,
                            key = rootButton.MenuKey,
                            type = rootButton.Type
                        });
                    }
                    else if (rootButton.Type.Equals("PIC_PHOTO_OR_ALBUM", StringComparison.OrdinalIgnoreCase))
                    {
                        //弹出拍照或者相册发图
                        bg.button.Add(new SinglePicPhotoOrAlbumButton()
                        {
                            name = rootButton.Name,
                            key = rootButton.MenuKey,
                            type = rootButton.Type
                        });
                    }
                    else if (rootButton.Type.Equals("PIC_SYSPHOTO", StringComparison.OrdinalIgnoreCase))
                    {
                        //弹出系统拍照发图
                        bg.button.Add(new SinglePicSysphotoButton()
                        {
                            name = rootButton.Name,
                            key = rootButton.MenuKey,
                            type = rootButton.Type
                        });
                    }
                    else if (rootButton.Type.Equals("PIC_WEIXIN", StringComparison.OrdinalIgnoreCase))
                    {
                        //弹出微信相册发图器
                        bg.button.Add(new SinglePicWeixinButton()
                        {
                            name = rootButton.Name,
                            key = rootButton.MenuKey,
                            type = rootButton.Type
                        });
                    }
                    else if (rootButton.Type.Equals("SCANCODE_PUSH", StringComparison.OrdinalIgnoreCase))
                    {
                        //扫码推事件
                        bg.button.Add(new SingleScancodePushButton()
                        {
                            name = rootButton.Name,
                            key = rootButton.MenuKey,
                            type = rootButton.Type
                        });
                    }
                    else
                    {
                        //扫码推事件且弹出“消息接收中”提示框
                        bg.button.Add(new SingleScancodeWaitmsgButton()
                        {
                            name = rootButton.Name,
                            key = rootButton.MenuKey,
                            type = rootButton.Type
                        });
                    }
                }
                #endregion

                #region 有子菜单
                else
                {
                    var subButton = new SubButton(rootButton.Name);
                    bg.button.Add(subButton);
                    foreach (var subSubButton in subbuttons)
                    {
                        if (subSubButton.Name == null)
                        {
                            continue; //没有设置菜单
                        }

                        if (subSubButton.Type.Equals("CLICK", StringComparison.OrdinalIgnoreCase)
                            && subSubButton.Id == 0)
                        {
                            throw new WeixinMenuException("单击按钮的key不能为空！");
                        }


                        if (subSubButton.Type.Equals("CLICK", StringComparison.OrdinalIgnoreCase))
                        {
                            //点击
                            subButton.sub_button.Add(new SingleClickButton()
                            {
                                name = subSubButton.Name,
                                key = subSubButton.MenuKey,
                                type = subSubButton.Type
                            });
                        }
                        else if (subSubButton.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase))
                        {
                            //URL
                            subButton.sub_button.Add(new SingleViewButton()
                            {
                                name = subSubButton.Name,
                                url = subSubButton.LinkUrl,
                                type = subSubButton.Type
                            });
                        }
                        else if (subSubButton.Type.Equals("LOCATION_SELECT", StringComparison.OrdinalIgnoreCase))
                        {
                            //弹出地理位置选择器
                            subButton.sub_button.Add(new SingleLocationSelectButton()
                            {
                                name = subSubButton.Name,
                                key = subSubButton.MenuKey,
                                type = subSubButton.Type
                            });
                        }
                        else if (subSubButton.Type.Equals("PIC_PHOTO_OR_ALBUM", StringComparison.OrdinalIgnoreCase))
                        {
                            //弹出拍照或者相册发图
                            subButton.sub_button.Add(new SinglePicPhotoOrAlbumButton()
                            {
                                name = subSubButton.Name,
                                key = subSubButton.MenuKey,
                                type = subSubButton.Type
                            });
                        }
                        else if (subSubButton.Type.Equals("PIC_SYSPHOTO", StringComparison.OrdinalIgnoreCase))
                        {
                            //弹出系统拍照发图
                            subButton.sub_button.Add(new SinglePicSysphotoButton()
                            {
                                name = subSubButton.Name,
                                key = subSubButton.MenuKey,
                                type = subSubButton.Type
                            });
                        }
                        else if (subSubButton.Type.Equals("PIC_WEIXIN", StringComparison.OrdinalIgnoreCase))
                        {
                            //弹出微信相册发图器
                            subButton.sub_button.Add(new SinglePicWeixinButton()
                            {
                                name = subSubButton.Name,
                                key = subSubButton.MenuKey,
                                type = subSubButton.Type
                            });
                        }
                        else if (subSubButton.Type.Equals("SCANCODE_PUSH", StringComparison.OrdinalIgnoreCase))
                        {
                            //扫码推事件
                            subButton.sub_button.Add(new SingleScancodePushButton()
                            {
                                name = subSubButton.Name,
                                key = subSubButton.MenuKey,
                                type = subSubButton.Type
                            });
                        }
                        else
                        {
                            //扫码推事件且弹出“消息接收中”提示框
                            subButton.sub_button.Add(new SingleScancodeWaitmsgButton()
                            {
                                name = subSubButton.Name,
                                key = subSubButton.MenuKey,
                                type = subSubButton.Type
                            });
                        }
                    }
                }
                #endregion
            }

            WxJsonResult result = null;
            try
            {
                result = CommonApi.CreateMenu(access_token, bg);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("更新MpID为{0}的菜单报错,原因：{1}", mpid, ex));
                try
                {
                    access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                    result = CommonApi.CreateMenu(access_token, bg);
                }
                catch (Exception e)
                {

                    throw new UserFriendlyException(e.Message);
                }

            }
            //if (result.errcode != ReturnCode.请求成功)
            //    result = CommonApi.CreateMenu(access_token, bg);
            //if (result.errcode != ReturnCode.请求成功)
            //    throw new WeixinException("更新MpID为" + mpid + "的菜单报错，原因：" + result.errmsg);

            return result;
        }
        #endregion

        #region 预览
        public async Task<SendResult> PreviewMatial(PreviewModel model)
        {
            Senparc.Weixin.MP.GroupMessageType previewType = Senparc.Weixin.MP.GroupMessageType.mpnews;
            if (model.MessageType == MpMessageType.mpnews.ToString())
                previewType = Senparc.Weixin.MP.GroupMessageType.mpnews;
            if (model.MessageType == MpMessageType.mpmultinews.ToString())
                previewType = Senparc.Weixin.MP.GroupMessageType.mpnews;
            if (model.MessageType == MpMessageType.text.ToString())
                previewType = Senparc.Weixin.MP.GroupMessageType.text;
            if (model.MessageType == MpMessageType.video.ToString())
                previewType = Senparc.Weixin.MP.GroupMessageType.video;
            if (model.MessageType == MpMessageType.voice.ToString())
                previewType = Senparc.Weixin.MP.GroupMessageType.video;
            if (model.MessageType == MpMessageType.image.ToString())
                previewType = Senparc.Weixin.MP.GroupMessageType.image;

            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = model.MpID });
            var access_token = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(account.AppId, account.AppSecret);
            SendResult result = null;
            try
            {
               

                result = GroupMessageApi.SendGroupMessagePreview(access_token, previewType, model.MediaID, model.OpenID,wxName:model.WxAccount);

            }
            catch
            {
                try
                {
                    access_token = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(account.AppId, account.AppSecret, true);
                    result = GroupMessageApi.SendGroupMessagePreview(access_token, previewType, model.MediaID, model.OpenID, wxName: model.WxAccount);
                }
                catch (Exception e)
                {

                    throw new UserFriendlyException(e.Message);
                }

            }
            return result;
        }
        #endregion

        #region 客服
        public async Task<CustomListInfo> GetCurstomerList(int mpId)
        {
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpId });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            CustomListInfo result = null;
            var urlFormat = string.Format(Config.ApiMpHost + "/cgi-bin/customservice/getkflist?access_token={0}", access_token.AsUrlData());
            try
            {
                
                result = await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync<CustomListInfo>(null, urlFormat, null, CommonJsonSendType.GET);
                //result = await CustomServiceApi.GetCustomBasicInfoAsync(access_token);
            }
            catch
            {
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    result = await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync<CustomListInfo>(null, urlFormat, null, CommonJsonSendType.GET);
                    //result = await CustomServiceApi.GetCustomBasicInfoAsync(access_token);
                }
                catch (Exception e)
                {

                    throw new UserFriendlyException(e.Message);
                }


            }
            return result;
        }

        public async Task<WxJsonResult> AddCustom(int mpId, string kfAccount, string nickName, string passWord, string inviteWx)
        {
            try
            {

                string _password = !string.IsNullOrWhiteSpace(passWord)?GetMD5(passWord):"";
                var account = _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpId }).Result;
                var access_token = _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result;
                WxJsonResult result = null;
                try
                {
                    result = await CustomServiceApi.AddCustomAsync(access_token, kfAccount, nickName, _password);
                    Logger.Error(result.errmsg);
                }
                catch
                {
                    access_token = _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true).Result;
                    try
                    {
                        result = await CustomServiceApi.AddCustomAsync(access_token, kfAccount, nickName, _password);
                        Logger.Error(result.errmsg);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e.Message);
                        throw new UserFriendlyException(e.Message);
                    }


                }
                await CustomServiceApi.InviteWorkerAsync(access_token, kfAccount, inviteWx);
                return result;
            }
            catch (Exception eh)
            {

                throw new UserFriendlyException(eh.Message);
            }

        }
        public async Task<WxJsonResult> UpdateCustom(int mpId, string kfAccount, string nickName, string passWord)
        {
            Logger.Info("加密前");
            string _password = !string.IsNullOrWhiteSpace(passWord) ?GetMD5(passWord):"";
            Logger.Info($"加密后密码为{_password}");
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpId });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            WxJsonResult result = null;
            try
            {
                result = await CustomServiceApi.UpdateCustomAsync(access_token, kfAccount, nickName, _password);
            }
            catch(Exception ex)
            {
                Logger.Info($"第一次错误：{ex.Message}");
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    result = await CustomServiceApi.UpdateCustomAsync(access_token, kfAccount, nickName, _password);
                }
                catch (Exception e)
                {
                    Logger.Info($"第二次错误：{e.Message}");
                    throw new UserFriendlyException(e.Message);
                }


            }
            return result;
        }
        public async Task<WxJsonResult> UploadCustomHeadimg(int mpId, string kfAccount, string filePath)
        {
            var tempProfilePicturePath = Path.Combine(_appFolders.TempFileDownloadFolder, filePath);
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpId });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            WxJsonResult result = null;
            try
            {
                result = await CustomServiceApi.UploadCustomHeadimgAsync(access_token, kfAccount, tempProfilePicturePath);
            }
            catch
            {
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    result = await CustomServiceApi.UploadCustomHeadimgAsync(access_token, kfAccount, tempProfilePicturePath);
                }
                catch (Exception e)
                {

                    throw new UserFriendlyException(e.Message);
                }


            }
            return result;
        }
        public async Task<WxJsonResult> DeleteCustom(int mpId, string kfAccount)
        {
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpId });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            WxJsonResult result = null;
            try
            {
                result = await CustomServiceApi.DeleteCustomAsync(access_token, kfAccount);

            }
            catch
            {
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                try
                {
                    result = await CustomServiceApi.DeleteCustomAsync(access_token, kfAccount);
                }
                catch (Exception e)
                {

                    throw new UserFriendlyException(e.Message);
                }


            }
            return result;
        }
        #endregion

        #region 带参二维码
        /// <summary>
        /// 申请带参二维码
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="eventKey"></param>
        /// <param name="channelType"></param>
        /// <param name="expireSeconds"></param>
        /// <returns></returns>
        public async Task<CreateQrCodeResult> GetQrCode(int mpid, string eventKey, string channelType, int expireSeconds = 0)
        {
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = mpid });
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            try
            {
                if (channelType == Senparc.Weixin.MP.QrCode_ActionName.QR_STR_SCENE.ToString())
                {
                    return await QrCodeApi.CreateAsync(access_token, expireSeconds, 0, Senparc.Weixin.MP.QrCode_ActionName.QR_STR_SCENE, eventKey);
                }
                else if (channelType == Senparc.Weixin.MP.QrCode_ActionName.QR_LIMIT_STR_SCENE.ToString())
                {
                    return await QrCodeApi.CreateAsync(access_token, expireSeconds, 0, Senparc.Weixin.MP.QrCode_ActionName.QR_LIMIT_STR_SCENE, eventKey);
                }
                else
                    return null;
            }
            catch 
            {
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret,true);
                try
                {
                    if (channelType == Senparc.Weixin.MP.QrCode_ActionName.QR_STR_SCENE.ToString())
                    {
                        return await QrCodeApi.CreateAsync(access_token, expireSeconds, 0, Senparc.Weixin.MP.QrCode_ActionName.QR_STR_SCENE, eventKey);
                    }
                    else if (channelType == Senparc.Weixin.MP.QrCode_ActionName.QR_LIMIT_STR_SCENE.ToString())
                    {
                        return await QrCodeApi.CreateAsync(access_token, expireSeconds, 0, Senparc.Weixin.MP.QrCode_ActionName.QR_LIMIT_STR_SCENE, eventKey);
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {

                    throw new UserFriendlyException(ex.Message); 
                }
            }
            
        }
        /// <summary>
        /// 保存二维码图片
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="name"></param>
        /// <param name="eventKey"></param>
        /// <param name="channelType"></param>
        /// <param name="expireSeconds"></param>
        /// <returns></returns>
        public async Task<QrCodeResult> SaveQrCode(int mpid, string name, string eventKey, string channelType, int expireSeconds = 0)
        {
           
            QrCodeResult resultModel = null;
            var result = await GetQrCode(mpid, eventKey, channelType, expireSeconds);
           
            if (result != null)
            {
              
                resultModel = new QrCodeResult();
                resultModel.Ticket = result.ticket;
                using (MemoryStream stream = new MemoryStream())
                {
                   
                    //await QrCodeApi.ShowQrCodeAsync(result.ticket, stream);
                   
                    resultModel.Url= QrCodeApi.GetShowQrCodeUrl(result.ticket);
                
                    //long length = stream.Length;
                    //if(length>0)
                    //{
                    //    using (System.Drawing.Image img = System.Drawing.Bitmap.FromStream(stream))
                    //    {
                    //        var fileName = string.Format("{0}_{1}.jpg", name, eventKey);
                    //        var tempProfilePicturePath = Path.Combine(_appFolders.QrCodeDownloadFolder, fileName);
                    //        resultModel.FilePath = tempProfilePicturePath;
                    //        img.Save(tempProfilePicturePath);
                    //    }

                          
                    //}
                    //else
                    //{
                    //    throw new UserFriendlyException("对不起，二维码流为空");
                    //}
                }
            }
            return resultModel;
        }
        #endregion

        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }
    }
    public class PostResult
    {
        public string media_id { get; set; }
    }

    public class QrCodeResult
    {
        public string Ticket { get; set; }
        public string Url { get; set; }
        public string FilePath { get; set; }
    }
}



