using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpMediaArticleGroups.Dto;
using Pb.Wechat.MpMediaArticleGroups.Exporting;
using Pb.Wechat.MpMediaArticles;
using Pb.Wechat.MpMediaArticles.Dto;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaArticleGroups
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class MpMediaArticleGroupAppService : AsyncCrudAppService<MpMediaArticleGroup, MpMediaArticleGroupDto, int, GetMpMediaArticleGroupsInput, MpMediaArticleGroupDto, MpMediaArticleGroupDto>, IMpMediaArticleGroupAppService
    {
        private readonly IRepository<MpAccount, int> _accountRepository;
        private readonly IRepository<MpMediaArticleGroupItem> _ItemRepository;
        private readonly IRepository<MpMediaArticle> _ArticleRepository;
        private readonly IMpMediaArticleGroupListExcelExporter _MpMediaArticleGroupListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IRepository<MpFan, int> _mpFanRepository;
        private readonly IWxMediaAppService _wxMediaAppService;

        public MpMediaArticleGroupAppService(IRepository<MpMediaArticleGroup, int> repository,
            IRepository<MpMediaArticleGroupItem> itemRepository,
            IRepository<MpMediaArticle> articleRepository,
            IMpMediaArticleGroupListExcelExporter MpMediaArticleGroupListExcelExporter,
            IRepository<MpAccount, int> accountRepository,
            IUserMpAppService userMpAppService, IRepository<MpFan, int> mpFanRepository, IWxMediaAppService wxMediaAppService) : base(repository)
        {
            _MpMediaArticleGroupListExcelExporter = MpMediaArticleGroupListExcelExporter;
            _ItemRepository = itemRepository;
            _ArticleRepository = articleRepository;
            _accountRepository = accountRepository;
            _userMpAppService = userMpAppService;
            _mpFanRepository = mpFanRepository;
            _wxMediaAppService = wxMediaAppService;
        }

        protected override IQueryable<MpMediaArticleGroup> CreateFilteredQuery(GetMpMediaArticleGroupsInput input)
        {
            List<int> groupIds = null;
            if(!string.IsNullOrWhiteSpace(input.SubTitle))
            {
                groupIds=_ItemRepository.GetAll().WhereIf(input.MpID != 0, c => c.MpID == input.MpID).Where(m =>m.IsDeleted==false && m.Title.Contains(input.SubTitle)).Select(m => m.GroupID).ToList();
            }

            return Repository.GetAll()
                .WhereIf(input.MpID != 0, c => c.MpID == input.MpID)
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Title))
                .WhereIf(groupIds!=null,c=>groupIds.Contains(c.Id));
        }
        public async Task<FileDto> GetListToExcel(GetMpMediaArticleGroupsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpMediaArticleGroupListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpMediaArticleGroupDto> GetModelByReplyTypeAsync(int id, int mpId)
        {
            CheckGetPermission();

            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(id != 0, c => c.Id == id)
                .WhereIf(mpId != 0, c => c.MpID == mpId));
            return MapToEntityDto(entity);
        }


        public async Task<MpMediaArticleGroupDto> Save(SaveMpMediaArticleGroupInput input)
        {
            CheckCreatePermission();
            var count = Repository.Count(m => m.IsDeleted == false && m.Name == input.Name && m.Id != input.Id);
            if (count>0)
                throw new UserFriendlyException("对不起，不能添加相同的名称的多图文信息。");
            // 保存主表信息  
            var MPID = await _userMpAppService.GetDefaultMpId();

            var data = new MpMediaArticleGroup { MpID = MPID };
            if (input.Id > 0)
            {
                data = await Repository.GetAsync(input.Id);
            }
            ObjectMapper.Map(input, data);

            List<int> intList = new List<int>();
            // 类型转化
            var idArray = input.ArticleIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < idArray.Length; i++)
            {
                intList.Add(int.Parse(idArray[i]));
            }
            //获取图文素材的所有数据
            var ArticleList = _ArticleRepository.GetAll().Where(m => intList.Contains(m.Id));


            List<NewsModel> gruopItems = new List<NewsModel>();
            foreach(var id in intList)
            {
                var itm = ArticleList.Where(m => m.Id == id).FirstOrDefault();
                gruopItems.Add(new NewsModel {
                    title = itm.Title,
                    author = itm.Author,
                    digest = itm.Description,
                    content = itm.Content,
                    content_source_url = itm.AUrl,
                    show_cover_pic = itm.ShowPic,
                    thumb_media_id = itm.PicMediaID
                });
            }
            if (!string.IsNullOrEmpty(data.MediaID))
            {
                for (int i = 0; i < gruopItems.Count; i++)
                {
                    await UpdateFileToWx(gruopItems[i], MPID, data.MediaID, i);
                }
            }
            else
                data.MediaID = await AddFileToWx(gruopItems.ToArray(), MPID, input.MediaID);
            data.LastModificationTime = DateTime.Now;
            var groupId = await Repository.InsertOrUpdateAndGetIdAsync(data);



            // 删除指定主表ID的所有子表数据
            await _ItemRepository.DeleteAsync(g => g.GroupID == groupId);

            // 循环插入子表数据
            var ids = input.ArticleIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < ids.Length; i++)
            {
                var article = ArticleList.FirstOrDefault(a => a.Id.ToString() == ids[i]);
                if (article != null)
                {
                    var GroupItem = new MpMediaArticleGroupItem
                    {
                        ArticleID = article.Id,
                        GroupID = groupId,
                        Title = article.Title,
                        MpID = data.MpID,
                        SortIndex = i + 1,
                        LastModificationTime = DateTime.Now
                    };
                    await _ItemRepository.InsertAsync(GroupItem);
                }
            }

            return MapToEntityDto(data);
        }


        public override Task Delete(EntityDto<int> input)
        {
            var model = Repository.Get(input.Id);

            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.MediaID))
                    _wxMediaAppService.DelFileFromWx(model.MpID, model.MediaID);
                _ItemRepository.DeleteAsync(g => g.GroupID == model.Id);
                return base.Delete(input);
            }
            else
                throw new UserFriendlyException("对不起，删除素材失败");
        }

        public async Task<PagedResultDto<MpMediaArticleGroupItemDto>> GetGroupItemList(EntityDto<int> input)
        {
            if (input.Id > 0)
            {
                var MPID = await _userMpAppService.GetDefaultMpId();
                var ItemList = await _ItemRepository.GetAllListAsync(c => c.IsDeleted == false && c.GroupID == input.Id && c.MpID == MPID);
                var ListSortIndex = ItemList.OrderBy(c => c.SortIndex); // 排序
                var List = ObjectMapper.Map<List<MpMediaArticleGroupItemDto>>(ListSortIndex);

                return new PagedResultDto<MpMediaArticleGroupItemDto>(List.Count, List);
            }
            else
            {
                return new PagedResultDto<MpMediaArticleGroupItemDto>();
            }
        }

        public async Task<List<MpMediaArticleGroupItemDto>> GetGroupItemListByGroupIds(List<int?> inputs)
        {
            if (inputs != null && inputs.Count > 0)
            {
                var MPID = await _userMpAppService.GetDefaultMpId();
                var ItemList = await _ItemRepository.GetAllListAsync(c => c.IsDeleted == false && inputs.Contains(c.GroupID) && c.MpID == MPID);
                var List = ItemList.OrderBy(c => c.GroupID).OrderBy(c => c.SortIndex).Select(c=> ObjectMapper.Map<MpMediaArticleGroupItemDto>(c)).ToList(); // 排序
                return List;
            }
            else
            {
                return null;
            }
        }

        public void PreviewMpArticle(MpMediaArticlePreviewDto input)
        {
            var fans = _mpFanRepository.GetAll().Where(m => m.NickName == input.NickName && m.IsDeleted == false).ToList();
            if (fans != null && fans.Count > 0)
            {
                foreach (var fan in fans)
                {
                    var openId = fan.OpenID;//粉丝OpenId；
                    var account = _accountRepository.FirstOrDefault(m => m.Id == input.MpID);
                    var access_token = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(account.AppId, account.AppSecret);
                    var result = GroupMessageApi.SendGroupMessagePreview(access_token, Senparc.Weixin.MP.GroupMessageType.mpnews, input.MediaID, openId);
                    if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
                        result = GroupMessageApi.SendGroupMessagePreview(access_token, Senparc.Weixin.MP.GroupMessageType.mpnews, input.MediaID, openId);
                }
            }
        }


        private async Task<string> AddFileToWx(NewsModel[] news, int mpid, string mediaID)
        {

            if (news == null || news.Length <= 0)
                return mediaID;
            var account = _accountRepository.FirstOrDefault(m => m.Id == mpid);
            var access_token = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(account.AppId, account.AppSecret);
            var result = await MediaApi.UploadNewsAsync(access_token, Senparc.Weixin.Config.TIME_OUT, news);
            return result.media_id;
        }

        private async Task<WxJsonResult> UpdateFileToWx(NewsModel news, int mpid, string mediaID, int index)
        {
            var account = _accountRepository.FirstOrDefault(m => m.Id == mpid);
            var access_token = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(account.AppId, account.AppSecret);
            return await MediaApi.UpdateForeverNewsAsync(access_token, mediaID, index, news);
        }


        public async Task<PagedResultDto<MediaArticleGroupOutput>> GetMultiArticlesDataList(GetMpMediaArticleGroupsInput input)
        {
            CheckGetAllPermission();
            var queryTemp = CreateFilteredQuery(input);
            queryTemp = ApplySorting(queryTemp, input);

            var datas = queryTemp.ToList();
            var groupIds = datas.Select(m => m.Id).ToList();
           
            var ItemList = await _ItemRepository.GetAllListAsync(c => c.IsDeleted == false && groupIds.Contains(c.GroupID) && c.MpID == input.MpID);
            var items = ItemList.OrderBy(c => c.GroupID).OrderBy(c => c.SortIndex).ToList(); // 排序

            var articles = items.Select(m => new { m.ArticleID, m.Id }).ToList();
            var articleIds = articles.Select(m => m.ArticleID).ToList();

            var itemResults = await _ArticleRepository.GetAllListAsync(m => articleIds.Contains(m.Id));

            var _messageType = MpMessageType.mpmultinews.ToString();
            var query = from t in datas
                        join tt in items on t.Id equals tt.GroupID
                        join tm in itemResults on tt.ArticleID equals tm.Id
                        select new MediaArticleGroupOutput
                        {
                            ArticleGroupID = t.Id,
                            ArticleID = tm.Id,
                            FilePathOrUrl = tm.FilePathOrUrl,
                            Id = t.Id,
                            LastModificationTime = t.LastModificationTime,
                            MessageType = _messageType,
                            Name = t.Name,
                            Title = tm.Title,
                            MediaID=t.MediaID,
                            MpID=t.MpID
                        };

            var totalCount = query.Count();

            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                query = query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
            }
            var resultList = query.ToList();
            var lastId = -1;
            foreach (var item in resultList)
            {
                if (item.Id != lastId)
                    item.RowSpan = resultList.Count(m => m.Id == item.Id);
                else
                    item.RowSpan = 0;
                lastId = item.Id;
            }
            return new PagedResultDto<MediaArticleGroupOutput>
            {
                TotalCount = totalCount,
                Items = resultList
            };
        }
    }

}



