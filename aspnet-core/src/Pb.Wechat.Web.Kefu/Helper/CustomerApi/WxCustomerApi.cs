using Microsoft.Ajax.Utilities;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Pb.Wechat.Web.Kefu
{
    public static class WxCustomerApi
    {
        private static string _urlFormat = Config.ApiMpHost + "/cgi-bin/message/custom/send?access_token={0}";
        #region 异步方法

        /// <summary>
        /// 【异步方法】发送文本信息
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId"></param>
        /// <param name="content"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="kfAccount">客服</param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendTextAsync(string accessTokenOrAppId, string openId, string content, int timeOut = Config.TIME_OUT, string kfAccount = "")
        {
            object data = null;
            if (string.IsNullOrEmpty(kfAccount))
            {
                data = new
                {
                    touser = openId,
                    msgtype = "text",
                    text = new
                    {
                        content = content
                    }

                };
            }
            else
            {
                data = new
                {
                    touser = openId,
                    msgtype = "text",
                    text = new
                    {
                        content = content
                    },
                    customservice = new
                    {
                        kf_account = kfAccount
                    }

                };
            }

            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {

                return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync(accessToken, _urlFormat, data, timeOut: timeOut);

            }, accessTokenOrAppId);
        }


        /// <summary>
        /// 【异步方法】发送图片消息
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="kfAccount">客服</param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendImageAsync(string accessTokenOrAppId, string openId, string mediaId, int timeOut = Config.TIME_OUT, string kfAccount = "")
        {
            object data = null;
            if (kfAccount.IsNullOrWhiteSpace())
            {
                data = new
                {
                    touser = openId,
                    msgtype = "image",
                    image = new
                    {
                        media_id = mediaId
                    }
                };
            }
            else
            {
                data = new
                {
                    touser = openId,
                    msgtype = "image",
                    image = new
                    {
                        media_id = mediaId
                    },
                    customservice = new
                    {
                        kf_account = kfAccount
                    }
                };
            }
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {

                return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync(accessToken, _urlFormat, data, timeOut: timeOut);

            }, accessTokenOrAppId);
        }

        /// <summary>
        /// 【异步方法】发送语音消息
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="kfAccount"></param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendVoiceAsync(string accessTokenOrAppId, string openId, string mediaId, int timeOut = Config.TIME_OUT, string kfAccount = "")
        {
            object data = null;
            if (kfAccount.IsNullOrWhiteSpace())
            {
                data = new
                {
                    touser = openId,
                    msgtype = "voice",
                    voice = new
                    {
                        media_id = mediaId
                    }
                };
            }
            else
            {
                data = new
                {
                    touser = openId,
                    msgtype = "voice",
                    voice = new
                    {
                        media_id = mediaId
                    },
                    customservice = new
                    {
                        kf_account = kfAccount
                    }
                };
            }
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {

                return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync(accessToken, _urlFormat, data, timeOut: timeOut);

            }, accessTokenOrAppId);
        }

        /// <summary>
        /// 【异步方法】发送视频消息
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="kfAccount">客服</param>
        /// <param name="thumb_media_id"></param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendVideoAsync(string accessTokenOrAppId, string openId, string mediaId, string title, string description, int timeOut = Config.TIME_OUT, string kfAccount = "", string thumb_media_id = "")
        {
            object data = null;
            if (kfAccount.IsNullOrWhiteSpace())
            {
                data = new
                {
                    touser = openId,
                    msgtype = "video",
                    video = new
                    {
                        media_id = mediaId,
                        thumb_media_id = thumb_media_id,
                        title = title,
                        description = description
                    }
                };
            }
            else
            {
                data = new
                {
                    touser = openId,
                    msgtype = "video",
                    video = new
                    {
                        media_id = mediaId,
                        thumb_media_id = thumb_media_id,
                        title = title,
                        description = description
                    },
                    customservice = new
                    {
                        kf_account = kfAccount
                    }
                };
            }
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {
                return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync(accessToken, _urlFormat, data, timeOut: timeOut);

            }, accessTokenOrAppId);
        }
        /// <summary>
        /// 【异步方法】发送音乐消息
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId"></param>
        /// <param name="title">音乐标题（非必须）</param>
        /// <param name="description">音乐描述（非必须）</param>
        /// <param name="musicUrl">音乐链接</param>
        /// <param name="hqMusicUrl">高品质音乐链接，wifi环境优先使用该链接播放音乐</param>
        /// <param name="thumbMediaId">视频缩略图的媒体ID</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="kfAccount">客服</param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendMusicAsync(string accessTokenOrAppId, string openId, string title, string description,
                                    string musicUrl, string hqMusicUrl, string thumbMediaId, int timeOut = Config.TIME_OUT, string kfAccount = "")
        {
            object data = null;
            if (kfAccount.IsNullOrWhiteSpace())
            {
                data = new
                {
                    touser = openId,
                    msgtype = "music",
                    music = new
                    {
                        title = title,
                        description = description,
                        musicurl = musicUrl,
                        hqmusicurl = hqMusicUrl,
                        thumb_media_id = thumbMediaId
                    }

                };
            }
            else
            {
                data = new
                {
                    touser = openId,
                    msgtype = "music",
                    music = new
                    {
                        title = title,
                        description = description,
                        musicurl = musicUrl,
                        hqmusicurl = hqMusicUrl,
                        thumb_media_id = thumbMediaId
                    },
                    customservice = new
                    {
                        kf_account = kfAccount
                    }

                };
            }
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {

                return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync(accessToken, _urlFormat, data, timeOut: timeOut);

            }, accessTokenOrAppId);
        }

        /// <summary>
        /// 【异步方法】发送图文消息
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId"></param>
        /// <param name="articles"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="kfAccount">客服</param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendNewsAsync(string accessTokenOrAppId, string openId, List<Article> articles, int timeOut = Config.TIME_OUT, string kfAccount = "")
        {
            object data = null;
            if (kfAccount.IsNullOrWhiteSpace())
            {
                data = new
                {
                    touser = openId,
                    msgtype = "news",
                    news = new
                    {
                        articles = articles.Select(z => new
                        {
                            title = z.Title,
                            description = z.Description,
                            url = z.Url,
                            picurl = z.PicUrl //图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80
                        }).ToList()
                    }

                };
            }
            else
            {
                data = new
                {
                    touser = openId,
                    msgtype = "news",
                    news = new
                    {
                        articles = articles.Select(z => new
                        {
                            title = z.Title,
                            description = z.Description,
                            url = z.Url,
                            picurl = z.PicUrl//图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80
                        }).ToList()
                    },
                    customservice = new
                    {
                        kf_account = kfAccount
                    }

                };
            }
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {

                return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync(accessToken, _urlFormat, data, timeOut: timeOut);

            }, accessTokenOrAppId);
        }


        /// <summary>
        /// 【异步方法】发送图文消息（点击跳转到图文消息页面）
        /// 图文消息条数限制在8条以内，注意，如果图文数超过8，则将会无响应。
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <param name="timeOut"></param>
        /// <param name="kfAccount">客服</param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendMpNewsAsync(string accessTokenOrAppId, string openId, string mediaId, int timeOut = Config.TIME_OUT, string kfAccount = "")
        {
            object data = null;
            if (kfAccount.IsNullOrWhiteSpace())
            {
                data = new
                {
                    touser = openId,
                    msgtype = "mpnews",
                    mpnews = new
                    {
                        media_id = mediaId
                    }
                };
            }
            else
            {
                data = new
                {
                    touser = openId,
                    msgtype = "mpnews",
                    mpnews = new
                    {
                        media_id = mediaId
                    },
                    customservice = new
                    {
                        kf_account = kfAccount
                    }
                };
            }
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {

                return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync(accessToken, _urlFormat, data, timeOut: timeOut);

            }, accessTokenOrAppId);
        }

        /// <summary>
        /// 【异步方法】发送卡券
        /// </summary>
        /// <param name="accessTokenOrAppId">AccessToken或AppId（推荐使用AppId，需要先注册）</param>
        /// <param name="openId"></param>
        /// <param name="cardId"></param>
        /// <param name="cardExt"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendCardAsync(string accessTokenOrAppId, string openId, string cardId, CardExt cardExt, int timeOut = Config.TIME_OUT)
        {
            return await ApiHandlerWapper.TryCommonApiAsync(async accessToken =>
            {
                var data = new
                {
                    touser = openId,
                    msgtype = "wxcard",
                    wxcard = new
                    {
                        card_id = cardId,
                        card_ext = cardExt
                    }
                };
                JsonSetting jsonSetting = new JsonSetting()
                {
                    TypesToIgnore = new List<System.Type>() { typeof(CardExt) }
                };

                return await Senparc.Weixin.CommonAPIs.CommonJsonSend.SendAsync(accessToken, _urlFormat, data, timeOut: timeOut, jsonSetting: jsonSetting);

            }, accessTokenOrAppId);
        }

        #endregion
    }
}