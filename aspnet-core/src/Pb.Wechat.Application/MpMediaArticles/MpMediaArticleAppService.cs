using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpMediaArticles.Dto;
using Pb.Wechat.MpMediaArticles.Exporting;
using Pb.Wechat.UserMps;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpFans;
using Pb.Wechat.WxMedias;
using Pb.Wechat.MpMediaImages;
using Pb.Wechat.MpArticleInsideImages;
using Abp.UI;
using System;

namespace Pb.Wechat.MpMediaArticles
{
    public class MpMediaArticleAppService : AsyncCrudAppService<MpMediaArticle, MpMediaArticleDto, int, GetMpMediaArticlesInput, MpMediaArticleDto, MpMediaArticleDto>, IMpMediaArticleAppService
    {
        private readonly IRepository<MpAccount, int> _accountRepository;
        private readonly IAppFolders _appFolders;
        private readonly IMpMediaArticleListExcelExporter _MpMediaArticleListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        //private readonly IMpFanAppService _mpFanAppService;
        private readonly IRepository<MpFan, int> _mpFanRepository;
        private readonly IWxMediaAppService _wxMediaAppService;
        private readonly IRepository<MpArticleInsideImage, int> _mpArticleInsideImageRepository;
        //private readonly IMpArticleInsideImageAppService _mpArticleInsideImageAppService;
        private readonly IRepository<MpMediaImage, int> _imageRepository;
        public MpMediaArticleAppService(IRepository<MpMediaArticle, int> repository, IMpMediaArticleListExcelExporter MpMediaArticleListExcelExporter, IUserMpAppService userMpAppService, IRepository<MpAccount, int> accountRepository, IAppFolders appFolders, IAccessTokenContainer accessTokenContainer, IRepository<MpFan, int> mpFanRepository, IWxMediaAppService wxMediaAppService, IRepository<MpMediaImage, int> imageRepository,
             IRepository<MpArticleInsideImage, int> mpArticleInsideImageRepository) : base(repository)
        {
            _MpMediaArticleListExcelExporter = MpMediaArticleListExcelExporter;
            _userMpAppService = userMpAppService;
            _accountRepository = accountRepository;
            _appFolders = appFolders;
            _accessTokenContainer = accessTokenContainer;
            _mpFanRepository = mpFanRepository;
            _wxMediaAppService = wxMediaAppService;
            _imageRepository = imageRepository;
            _mpArticleInsideImageRepository = mpArticleInsideImageRepository;
        }

        protected override IQueryable<MpMediaArticle> CreateFilteredQuery(GetMpMediaArticlesInput input)
        {

            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                 .WhereIf(!input.MediaID.IsNullOrWhiteSpace(), c => c.MediaID.Contains(input.MediaID))
                  .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Title))
                   .WhereIf(!input.Content.IsNullOrWhiteSpace(), c => c.Content.Contains(input.Content))
                   .WhereIf(!input.Description.IsNullOrWhiteSpace(), c => c.Description.Contains(input.Description))
                   .WhereIf(!input.Author.IsNullOrWhiteSpace(), c => c.Author.Contains(input.Author))
                   .WhereIf(!input.PicFileID.IsNullOrWhiteSpace(), c => c.PicFileID.Contains(input.PicFileID))
                   .WhereIf(!input.PicMediaID.IsNullOrWhiteSpace(), c => c.PicMediaID.Contains(input.PicMediaID));
        }
        public override Task Delete(EntityDto<int> input)
        {
            var model = Repository.Get(input.Id);
            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.MediaID))
                    _wxMediaAppService.DelFileFromWx(model.MpID, model.MediaID);
                return base.Delete(input);
            }
            else
                throw new UserFriendlyException("对不起，删除素材失败");

        }
        //public override async Task<MpMediaArticleDto> Create(MpMediaArticleDto input)
        //{


        //    #region 曾经代码
        //    //input.MpID = await _userMpAppService.GetDefaultMpId();
        //    //input.WxContent = await GetWxContent(input.MpID, input.Content, input.SavePath);
        //    ////替换富文本内容的src，尚未完成
        //    //input.Content = input.Content.Replace(string.Format("src=\"{0}", input.SavePath), string.Format("src=\"{0}{1}", input.HostName, input.SavePath));

        //    //input.LastModificationTime = DateTime.Now;

        //    //if (!string.IsNullOrWhiteSpace(input.PicMediaID))
        //    //{

        //    //    var imageModel = await _imageRepository.FirstOrDefaultAsync(m => m.MediaID == input.PicMediaID);
        //    //    if (!string.IsNullOrWhiteSpace(input.PicFileID))
        //    //    {
        //    //        if (input.PicFileID != imageModel.FileID)
        //    //        {
        //    //            input.PicMediaID = await _wxMediaAppService.UploadMedia(input.PicFileID, "");
        //    //            await _imageRepository.InsertAsync(new MpMediaImage
        //    //            {
        //    //                FileID = input.PicFileID,
        //    //                FilePathOrUrl = input.FilePathOrUrl,
        //    //                MpID = input.MpID,
        //    //                Name = input.Title + "封面图片",
        //    //                MediaID = input.PicMediaID,
        //    //                LastModificationTime=DateTime.Now
        //    //            });

        //    //        }

        //    //    }
        //    //    else
        //    //    {
        //    //        input.PicFileID = imageModel.FileID;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    if (!string.IsNullOrWhiteSpace(input.PicFileID))
        //    //    {
        //    //        input.PicMediaID = await _wxMediaAppService.UploadMedia(input.PicFileID, "");
        //    //        await _imageRepository.InsertAsync(new MpMediaImage
        //    //        {
        //    //            FileID = input.PicFileID,
        //    //            FilePathOrUrl = input.FilePathOrUrl,
        //    //            MpID = input.MpID,
        //    //            Name = input.Title + "封面图片",
        //    //            MediaID = input.PicMediaID,
        //    //            LastModificationTime=DateTime.Now
        //    //        });


        //    //    }
        //    //}
        //    //input.MediaID = await AddFileToWx(input, input.MpID, input.MediaID);
        //    #endregion

        //    CheckCreatePermission();
        //    var entity = MapToEntity(input);
        //    Repository.Insert(entity);
        //    return MapToEntityDto(entity);
        //}
        //public override async Task<MpMediaArticleDto> Update(MpMediaArticleDto input)
        //{
        //    input.WxContent = await GetWxContent(input.MpID, input.Content, input.SavePath);
        //    input.Content = input.Content.Replace(string.Format("src=\"{0}", input.SavePath), string.Format("src=\"{0}{1}", input.HostName, input.SavePath));

        //    if (!string.IsNullOrWhiteSpace(input.PicMediaID))
        //    {

        //        var imageModel = await _imageRepository.FirstOrDefaultAsync(m => m.MediaID == input.PicMediaID);
        //        if (!string.IsNullOrWhiteSpace(input.PicFileID))
        //        {
        //            if (input.PicFileID != imageModel.FileID)
        //            {
        //                input.PicMediaID = await _wxMediaAppService.UploadMedia(input.PicFileID, "");
        //                await _imageRepository.InsertAsync(new MpMediaImage
        //                {
        //                    FileID = input.PicFileID,
        //                    FilePathOrUrl = input.FilePathOrUrl,
        //                    MpID = input.MpID,
        //                    Name = input.Title + "封面图片",
        //                    MediaID = input.PicMediaID,
        //                    LastModificationTime=DateTime.Now
        //                });

        //            }

        //        }
        //        else
        //        {
        //            input.PicFileID = imageModel.FileID;
        //        }
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrWhiteSpace(input.PicFileID))
        //        {
        //            input.PicMediaID = await _wxMediaAppService.UploadMedia(input.PicFileID, "");
        //            await _imageRepository.InsertAsync(new MpMediaImage
        //            {
        //                FileID = input.PicFileID,
        //                FilePathOrUrl = input.FilePathOrUrl,
        //                MpID = input.MpID,
        //                Name = input.Title + "封面图片",
        //                MediaID = input.PicMediaID,
        //                LastModificationTime=DateTime.Now
        //            });


        //        }
        //    }
        //    await UpdateFileToWx(input, input.MpID, input.MediaID);
        //    CheckUpdatePermission();
        //    var entity = Repository.Get(input.Id);
        //    MapToEntity(input, entity);
        //    return MapToEntityDto(entity);

        //}
        public async Task<FileDto> GetListToExcel(GetMpMediaArticlesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpMediaArticleListExcelExporter.ExportToFile(dtos);
        }
        /// <summary>
        /// 地址转换
        /// </summary>
        /// <param name="mpId"></param>
        /// <param name="content"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<string> GetWxContent(int mpId, string content, string key)
        {
            var _content = content;
            var array = content.Split("src=\"");

            foreach (var item in array)
            {
                if (item.StartsWith(key))
                {
                    var yStr = item.Substring(0, item.IndexOf("\""));
                    var _yStr = yStr.Substring(1).Replace("\"", "").Replace("/", "\\");
                    //var tempPath = Path.Combine(_appFolders.TempFileDownloadFolder, yStr);

                    var wxUrl = "";
                    var rModel = await _mpArticleInsideImageRepository.FirstOrDefaultAsync(m => m.MpID == mpId && m.LocalImageUrl == yStr);
                    //_mpArticleInsideImageAppService.GetFirstOrDefault(new MpArticleInsideImages.Dto.GetMpArticleInsideImagesInput { MpID = mpId, LocalImageUrl = yStr }).Result;
                    if (rModel == null)
                    {
                        wxUrl = await _wxMediaAppService.UploadArticleInImage(mpId, _yStr);

                        await _mpArticleInsideImageRepository.InsertAsync(new MpArticleInsideImage
                        {
                            MpID = mpId,
                            LocalImageUrl = yStr,
                            WxImageUrl = wxUrl
                        });
                        //_mpArticleInsideImageAppService.Create(new MpArticleInsideImages.Dto.MpArticleInsideImageDto
                        //{
                        //    MpID = mpId,
                        //    LocalImageUrl = yStr,
                        //    WxImageUrl = wxUrl
                        //});
                    }
                    else
                        wxUrl = rModel.WxImageUrl;
                    _content = _content.Replace(yStr, wxUrl);
                }

            }
            return _content;
        }
        private async Task<string> AddFileToWx(MpMediaArticleDto article, int mpid, string mediaID)
        {

            if (article == null)
                return mediaID;
            var account = _accountRepository.FirstOrDefault(m => m.Id == mpid);

            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            //var access_token = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(account.AppId, account.AppSecret);

            var news = new NewsModel()
            {
                title = article.Title,
                author = article.Author,
                digest = article.Description,
                content = article.WxContent,
                content_source_url = article.AUrl,
                show_cover_pic = article.ShowPic,
                thumb_media_id = article.PicMediaID,
                need_open_comment = article.EnableComment,
                only_fans_can_comment = article.OnlyFansComment

            };
            return MediaApi.UploadNews(access_token, Senparc.Weixin.Config.TIME_OUT, news).media_id;

        }
        private async Task<WxJsonResult> UpdateFileToWx(MpMediaArticleDto article, int mpid, string mediaID)
        {

            if (article == null)
                return new WxJsonResult { errcode = Senparc.Weixin.ReturnCode.POST的数据包为空, errmsg = "实体对象不存在", P2PData = null };
            var account = _accountRepository.FirstOrDefault(m => m.Id == mpid);
            //var access_token = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(account.AppId, account.AppSecret);
            var access_token = _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result;
            var news = new NewsModel()
            {
                title = article.Title,
                author = article.Author,
                digest = article.Description,
                content = article.WxContent,
                content_source_url = article.AUrl,
                show_cover_pic = article.ShowPic,
                thumb_media_id = article.PicMediaID,
                need_open_comment = article.EnableComment,
                only_fans_can_comment = article.OnlyFansComment
            };
            WxJsonResult result = null;

            return result = await MediaApi.UpdateForeverNewsAsync(access_token, mediaID, 0, news, Senparc.Weixin.Config.TIME_OUT);

        }

        public void PreviewMpArticle(MpMediaArticlePreviewDto input)
        {
            var fans = _mpFanRepository.GetAll().Where(m => m.NickName == input.NickName && m.IsDeleted == false).ToList();
            /*_mpFanAppService.FindByNickName(input.NickName);*/
            if (fans != null && fans.Count > 0)
            {
                foreach (var fan in fans)
                {
                    var openId = fan.OpenID;//粉丝OpenId；
                    var account = _accountRepository.FirstOrDefault(m => m.Id == input.MpID);

                    var access_token = _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result;
                    var result = GroupMessageApi.SendGroupMessagePreview(access_token, Senparc.Weixin.MP.GroupMessageType.mpnews, input.MediaID, openId);
                    if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
                        result = GroupMessageApi.SendGroupMessagePreview(access_token, Senparc.Weixin.MP.GroupMessageType.mpnews, input.MediaID, openId);
                }

            }
        }

        public async Task<List<MpMediaArticleDto>> GetList()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.MpID == mpid))).Select(MapToEntityDto).ToList();
        }

        public async Task Save(MpMediaArticleDto input)
        {
            await this.Update(input);
        }

        public async Task<List<MpMediaArticleDto>> GetListByIds(List<int> Ids)
        {
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => Ids.Contains(m.Id)))).Select(MapToEntityDto).ToList();
        }

        public async Task<string> UpdateWxContentString(string content,string articleGuid, string domain = null)
        {
            var _content = content;
           var insideImages=await  _mpArticleInsideImageRepository.GetAllListAsync(m => m.IsDeleted == false && m.ArticleGrid == articleGuid);

            foreach(var image in insideImages)
            {
                if (!string.IsNullOrWhiteSpace(domain))
                {
                    var _tempStr = image.LocalImageUrl.Replace(domain,"");
                    _content = _content.Replace(_tempStr, image.WxImageUrl);
                }
               
                _content = _content.Replace(image.LocalImageUrl, image.WxImageUrl);
            }
            return _content;
        }
    }
}
