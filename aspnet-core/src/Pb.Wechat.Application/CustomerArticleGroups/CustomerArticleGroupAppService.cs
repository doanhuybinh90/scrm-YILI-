using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerArticleGroupItems;
using Pb.Wechat.CustomerArticleGroups.Dto;
using Pb.Wechat.CustomerArticleGroups.Exporting;
using Pb.Wechat.CustomerArticles;
using Pb.Wechat.UserMps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.CustomerServiceResponseTexts;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Pb.Wechat.MpEvents.Dto;

namespace Pb.Wechat.CustomerArticleGroups
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class CustomerArticleGroupAppService : AsyncCrudAppService<CustomerArticleGroup, CustomerArticleGroupDto, int, GetCustomerArticleGroupsInput, CustomerArticleGroupDto, CustomerArticleGroupDto>, ICustomerArticleGroupAppService
    {
        private readonly IRepository<CustomerArticle, int> _selfArticles;
        private readonly IRepository<CustomerArticleGroupItem, int> _CustomerArticleGroupItem;
        private readonly IUserMpAppService _userMpAppService;
        private readonly ICustomerArticleGroupListExcelExporter _CustomerArticleGroupListExcelExporter;
        private readonly IRepository<CustomerServiceResponseText, int> _cusRepository;
        public CustomerArticleGroupAppService(IRepository<CustomerArticleGroup, int> repository, ICustomerArticleGroupListExcelExporter CustomerArticleGroupListExcelExporter, IUserMpAppService userMpAppService, IRepository<CustomerArticleGroupItem, int> CustomerArticleGroupItem, IRepository<CustomerArticle, int> selfArticles, IRepository<CustomerServiceResponseText, int> cusRepository) : base(repository)
        {
            _CustomerArticleGroupListExcelExporter = CustomerArticleGroupListExcelExporter;
            _userMpAppService = userMpAppService;
            _CustomerArticleGroupItem = CustomerArticleGroupItem;
            _selfArticles = selfArticles;
            _cusRepository = cusRepository;
        }

        protected override IQueryable<CustomerArticleGroup> CreateFilteredQuery(GetCustomerArticleGroupsInput input)
        {

            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Title));
        }
        public async Task<FileDto> GetListToExcel(GetCustomerArticleGroupsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _CustomerArticleGroupListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CustomerArticleGroupDto> GetModelByReplyTypeAsync(int id, int mpId)
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
            var model = Repository.Get(input.Id);
            _CustomerArticleGroupItem.Delete(m => m.GroupID == input.Id);
            _cusRepository.Delete(m => m.MartialId == model.Id);
            return base.Delete(input);
        }
        public async Task<bool> Save(CustomerArticleGroupDto input)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();

            var count = Repository.Count(m => m.IsDeleted == false && m.Name == input.Name && m.Id != input.Id && m.MpID == input.MpID);
            if (count > 0)
                throw new UserFriendlyException("对不起，不能添加相同的名称的多图文信息。");

            if (input.MpID == 0)
                input.MpID = mpid;
            int groupId = 0;
            string mediaId = "";
            if (input.Id != 0)
            {
                groupId = input.Id;
                var upResult=await base.Update(input);
                mediaId = upResult.MediaID;
                var updateModel = await _cusRepository.FirstOrDefaultAsync(m => m.MartialId == groupId);
                updateModel.Title = input.Name;
                updateModel.LastModificationTime = DateTime.Now;
                updateModel.MediaId = mediaId;
                updateModel.TypeId = input.TypeId;
                updateModel.TypeName = input.TypeName;
                updateModel.ResponseText = input.Name;
                await _cusRepository.UpdateAsync(updateModel);
                
            }
            else
            {
                mediaId = Guid.NewGuid().ToString();
                groupId = await base.Repository.InsertAndGetIdAsync(new CustomerArticleGroup { Name = input.Name, MpID = input.MpID, LastModificationTime = DateTime.Now, MediaID = mediaId });
                await _cusRepository.InsertAsync(new CustomerServiceResponseText
                {
                    Title = input.Name,
                    CreationTime = DateTime.Now,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now,
                    MediaId = mediaId,
                    MpID = input.MpID,
                    PreviewImgUrl = "",
                    MartialId = groupId,
                    ResponseType = ResponseType.common.ToString(),
                    ReponseContentType = (int)CustomerServiceMsgType.mpmultinews,
                    TypeId = input.TypeId,
                    TypeName = input.TypeName,
                    ResponseText = input.Name
                });
            }
               
            var ids = input.ItemIds.Split(',').ToList();
            var intList = new List<int>();
            foreach (var id in ids)
            {
                intList.Add(int.Parse(id));
            }
            var models = await _selfArticles.GetAllListAsync(m => intList.Contains(m.Id));
            List<CustomerArticle> sortModels = new List<CustomerArticle>();
            foreach (var id in intList)
            {
                sortModels.Add(models.Where(m => m.Id == id).FirstOrDefault());
            }
            List<CustomerArticleGroupItem> datas = new List<CustomerArticleGroupItem>();
            int i = 1;
            foreach (var model in sortModels)
            {

                var data = new CustomerArticleGroupItem
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
            //var result=_CustomerArticleGroupItemAppService.SaveItems(datas);

            if (datas != null && datas.Count > 0)
            {
                //var groupId = datas.First().GroupID;
                await _CustomerArticleGroupItem.DeleteAsync(m => m.GroupID == groupId);
                foreach (var data in datas)
                {
                    await _CustomerArticleGroupItem.InsertAsync(data);
                }
                return true;
            }
            return false;
            //return result;

        }


        public async Task<PagedResultDto<CustomerArticleGroupOutput>> GetMultiArticlesDataList(GetCustomerArticleGroupsInput input)
        {
            CheckGetAllPermission();
            var queryTemp = CreateFilteredQuery(input);
            queryTemp = ApplySorting(queryTemp, input);

            var datas = queryTemp.ToList();
            var groupIds = datas.Select(m => m.Id).ToList();

            var ItemList = await _CustomerArticleGroupItem.GetAllListAsync(c => c.IsDeleted == false && groupIds.Contains(c.GroupID) && c.MpID == input.MpID);
            var items = ItemList.OrderBy(c => c.GroupID).OrderBy(c => c.SortIndex).ToList(); // 排序

            var articles = items.Select(m => new { m.ArticleID, m.Id }).ToList();
            var articleIds = articles.Select(m => m.ArticleID).ToList();

            var itemResults = await _selfArticles.GetAllListAsync(m => articleIds.Contains(m.Id));

            var _messageType = MpMessageType.mpmultinews.ToString();
            var query = from t in datas
                        join tt in items on t.Id equals tt.GroupID
                        join tm in itemResults on tt.ArticleID equals tm.Id
                        select new CustomerArticleGroupOutput
                        {
                            ArticleGroupID = t.Id,
                            ArticleID = tm.Id,
                            FilePathOrUrl = tm.FilePathOrUrl,
                            Id = t.Id,
                            LastModificationTime = t.LastModificationTime,
                            MessageType = _messageType,
                            Name = t.Name,
                            Title = tm.Title,
                            MediaID = t.MediaID,
                            MpID = t.MpID,
                            AUrl=tm.AUrl,
                            Description=tm.Description
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
            return new PagedResultDto<CustomerArticleGroupOutput>
            {
                TotalCount = totalCount,
                Items = resultList
            };
        }
    }
}
