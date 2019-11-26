using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpSelfArticleGroupItems;
using Pb.Wechat.MpSelfArticleGroups.Dto;
using Pb.Wechat.MpSelfArticleGroups.Exporting;
using Pb.Wechat.MpSelfArticles;
using Pb.Wechat.UserMps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSelfArticleGroups
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class MpSelfArticleGroupAppService : AsyncCrudAppService<MpSelfArticleGroup, MpSelfArticleGroupDto, int, GetMpSelfArticleGroupsInput, MpSelfArticleGroupDto, MpSelfArticleGroupDto>, IMpSelfArticleGroupAppService
    {
        private readonly IRepository<MpSelfArticle, int> _selfArticles;
        private readonly IRepository<MpSelfArticleGroupItem, int> _mpSelfArticleGroupItem;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpSelfArticleGroupListExcelExporter _MpSelfArticleGroupListExcelExporter;
        public MpSelfArticleGroupAppService(IRepository<MpSelfArticleGroup, int> repository, IMpSelfArticleGroupListExcelExporter MpSelfArticleGroupListExcelExporter, IUserMpAppService userMpAppService, IRepository<MpSelfArticleGroupItem, int> mpSelfArticleGroupItem, IRepository<MpSelfArticle, int> selfArticles) : base(repository)
        {
            _MpSelfArticleGroupListExcelExporter = MpSelfArticleGroupListExcelExporter;
            _userMpAppService = userMpAppService;
            _mpSelfArticleGroupItem = mpSelfArticleGroupItem;
            _selfArticles = selfArticles;
        }

        protected override IQueryable<MpSelfArticleGroup> CreateFilteredQuery(GetMpSelfArticleGroupsInput input)
        {

            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Title));
        }
        public async Task<FileDto> GetListToExcel(GetMpSelfArticleGroupsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpSelfArticleGroupListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpSelfArticleGroupDto> GetModelByReplyTypeAsync(int id, int mpId)
        {
            CheckGetPermission();

            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(id != 0, c => c.Id == id)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

        public override Task Delete(EntityDto<int> input)
        {
            _mpSelfArticleGroupItem.Delete(m => m.GroupID == input.Id);
            return base.Delete(input);
        }
        public async Task<bool> Save(MpSelfArticleGroupDto input)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();

            var count = Repository.Count(m => m.IsDeleted == false && m.Name == input.Name && m.Id != input.Id && m.MpID == input.MpID);
            if (count > 0)
                throw new UserFriendlyException("对不起，不能添加相同的名称的多图文信息。");

            if (input.MpID == 0)
                input.MpID = mpid;
            int groupId = 0;
            if (input.Id != 0)
            {
                groupId = input.Id;
                await base.Update(input);
            }
            else
                groupId = await base.Repository.InsertAndGetIdAsync(new MpSelfArticleGroup { Name = input.Name, MpID = input.MpID, LastModificationTime = DateTime.Now });
            var ids = input.ItemIds.Split(',').ToList();
            var intList = new List<int>();
            foreach (var id in ids)
            {
                intList.Add(int.Parse(id));
            }
            var models = await _selfArticles.GetAllListAsync(m => intList.Contains(m.Id));
            List<MpSelfArticle> sortModels = new List<MpSelfArticle>();
            foreach (var id in intList)
            {
                sortModels.Add(models.Where(m => m.Id == id).FirstOrDefault());
            }
            List<MpSelfArticleGroupItem> datas = new List<MpSelfArticleGroupItem>();
            int i = 1;
            foreach (var model in sortModels)
            {

                var data = new MpSelfArticleGroupItem
                {
                    MpID = input.MpID,
                    ArticleID = model.Id,
                    GroupID = groupId,
                    SortIndex = i,
                    Title = model.Title
                };
                datas.Add(data);
                i++;
            }
            //var result=_mpSelfArticleGroupItemAppService.SaveItems(datas);

            if (datas != null && datas.Count > 0)
            {
                //var groupId = datas.First().GroupID;
                await _mpSelfArticleGroupItem.DeleteAsync(m => m.GroupID == groupId);
                foreach (var data in datas)
                {
                    await _mpSelfArticleGroupItem.InsertAsync(data);
                }
                return true;
            }
            return false;
            //return result;

        }

        public async Task<PagedResultDto<MpSelfArticleGroupOutput>> GetMultiArticlesDataList(GetMpSelfArticleGroupsInput input)
        {
            CheckGetAllPermission();
            var queryTemp = CreateFilteredQuery(input);
            queryTemp = ApplySorting(queryTemp, input);

            var datas = queryTemp.ToList();
            var groupIds = datas.Select(m => m.Id).ToList();

            var ItemList = await _mpSelfArticleGroupItem.GetAllListAsync(c => c.IsDeleted == false && groupIds.Contains(c.GroupID) && c.MpID == input.MpID);
            var items = ItemList.OrderBy(c => c.GroupID).OrderBy(c => c.SortIndex).ToList(); // 排序

            var articles = items.Select(m => new { m.ArticleID, m.Id }).ToList();
            var articleIds = articles.Select(m => m.ArticleID).ToList();

            var itemResults = await _selfArticles.GetAllListAsync(m => articleIds.Contains(m.Id));

            var _messageType = MpMessageType.mpmultinews.ToString();
            var query = from t in datas
                        join tt in items on t.Id equals tt.GroupID
                        join tm in itemResults on tt.ArticleID equals tm.Id
                        select new MpSelfArticleGroupOutput
                        {
                            ArticleGroupID = t.Id,
                            ArticleID = tm.Id,
                            FilePathOrUrl = tm.FilePathOrUrl,
                            Id = t.Id,
                            LastModificationTime = t.LastModificationTime,
                            MessageType = _messageType,
                            Name = t.Name,
                            Title = tm.Title,
                            MpID = t.MpID,
                            AUrl = tm.AUrl,
                            Description = tm.Description
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
            return new PagedResultDto<MpSelfArticleGroupOutput>
            {
                TotalCount = totalCount,
                Items = resultList
            };
        }
    }
}
