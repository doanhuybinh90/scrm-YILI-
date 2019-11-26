using Abp.Application.Services;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMessages.Dto;
using Pb.Wechat.MpMessages.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Pb.Wechat.UserMps;
using Pb.Wechat.MpGroups;
using Pb.Wechat.MpMediaArticleGroups;
using Pb.Wechat.MpMediaArticles;
using Pb.Wechat.MpGroupItems;
using Abp.UI;
using Pb.Wechat.MpGroups.Dto;

namespace Pb.Wechat.MpMessages
{
    //[AbpAuthorize(AppPermissions.Pages_MpMessages)]
    public class MpMessageAppService : AsyncCrudAppService<MpMessage, MpMessageDto, int, GetMpMessagesInput, MpMessageDto, MpMessageDto>, IMpMessageAppService
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpMessageListExcelExporter _mpAccountListExcelExporter;
        private readonly IRepository<MpGroup, int> _mpGroupRepository;
        private readonly IRepository<MpMediaArticleGroup, int> _mpMediaArticleGroup;
        private readonly IRepository<MpMediaArticleGroupItem, int> _ItemRepository;
        private readonly IRepository<MpMediaArticle, int> _mpMediaArticleRepository;
        private readonly IRepository<MpGroupItem, int> _mpGroupItemRepository;
        public MpMessageAppService(IRepository<MpMessage, int> repository, IMpMessageListExcelExporter mpAccountListExcelExporter, IUserMpAppService userMpAppService, IRepository<MpGroup, int> mpGroupRepository, IRepository<MpMediaArticleGroup, int> mpMediaArticleGroup, IRepository<MpMediaArticle, int> mpMediaArticleRepository, IRepository<MpMediaArticleGroupItem, int> ItemRepository, IRepository<MpGroupItem, int> mpGroupItemRepository) : base(repository)
        {
            _mpAccountListExcelExporter = mpAccountListExcelExporter;
            _userMpAppService = userMpAppService;
            _mpGroupRepository = mpGroupRepository;
            _mpMediaArticleGroup = mpMediaArticleGroup;
            _mpMediaArticleRepository = mpMediaArticleRepository;
            _ItemRepository = ItemRepository;
            _mpGroupItemRepository = mpGroupItemRepository;
        }

        protected override IQueryable<MpMessage> CreateFilteredQuery(GetMpMessagesInput input)
        {

            var inputtype = input.MessageTypeX == null ? "" : input.MessageTypeX.ToString();

            return Repository.GetAll()
                .WhereIf(input.MpID != 0, c => c.MpID == input.MpID)
                .WhereIf(input.MessageTypeX != null, c => c.MessageType == inputtype)
                .WhereIf(!string.IsNullOrWhiteSpace(input.WxMsgID), c => c.WxMsgID.Contains(input.WxMsgID))
                 ;
        }


        public async Task<PagedResultDto<MpMessageOtherDataListOutput>> GetOtherDataList(GetMpMessagesInput input)
        {
            CheckGetAllPermission();
            var query = CreateFilteredQuery(input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<MpMessageOtherDataListOutput>(
                totalCount,
                entities.Select(MapToMpMessageOtherData).ToList()
            );
        }

        public async Task<PagedResultDto<MpMessageMultiArticlesDataListOutput>> GetMultiArticlesDataList(GetMpMessagesInput input)
        {
            CheckGetAllPermission();
            var queryTemp = CreateFilteredQuery(input);
            queryTemp = ApplySorting(queryTemp, input);

            var datas = queryTemp.ToList();
            var groupIds = datas.Select(m => m.ArticleGroupID).ToList();

            var ItemList = await _ItemRepository.GetAllListAsync(c => c.IsDeleted == false && groupIds.Contains(c.GroupID) && c.MpID == input.MpID);
            var items = ItemList.OrderBy(c => c.GroupID).OrderBy(c => c.SortIndex).ToList(); // 排序

            var articles = items.Select(m => new { m.ArticleID, m.Id }).ToList();
            var articleIds = articles.Select(m => m.ArticleID).ToList();

            var itemResults = await _mpMediaArticleRepository.GetAllListAsync(m => articleIds.Contains(m.Id));


            var query = from t in datas
                        join tt in items on t.ArticleGroupID equals tt.GroupID
                        join tm in itemResults on tt.ArticleID equals tm.Id
                        select new MpMessageMultiArticlesDataListOutput
                        {
                            ArticleGroupID = t.ArticleGroupID,
                            ArticleID = tm.Id,
                            FilePathOrUrl = tm.FilePathOrUrl,
                            Id = t.Id,
                            LastModificationTime = t.LastModificationTime,
                            MessageType = t.MessageType,
                            Name = t.ArticleGroupName,
                            Title = tm.Title,
                            ExecTaskTime = t.ExecTaskTime,
                            FailCount = t.FailCount,
                            FinishDate = t.FinishDate,
                            IsTask = t.IsTask,
                            SendCount = t.SendCount,
                            SendState = t.SendState,
                            State = t.State,
                            SuccessCount = t.SuccessCount
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
            return new PagedResultDto<MpMessageMultiArticlesDataListOutput>
            {
                TotalCount = totalCount,
                Items = resultList
            };
        }

        public async Task<FileDto> GetListToExcel(GetMpMessagesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpAccountListExcelExporter.ExportToFile(dtos);
        }

        private async Task<string> GetGroupName(int mpid, int groupId)
        {
            var list = new List<int>();
            var group = await AsyncQueryableExecuter.FirstOrDefaultAsync(_mpGroupRepository.GetAll().Where(m => m.IsDeleted == false && m.MpID == mpid && m.Id == groupId));
            return group == null ? "" : group.Name;
        }
        public override async Task<MpMessageDto> Create(MpMessageDto input)
        {
            if (!input.GroupID.HasValue)
                input.GroupID =0;
            input.WxMsgID = Guid.NewGuid().ToString().Replace("-", "");
            input.GroupName =await GetGroupName(input.MpID, input.GroupID.Value);
            var item = _mpGroupItemRepository.FirstOrDefault(m => m.IsDeleted == false && m.ParentID == input.GroupID);
            if (item == null)
            {
                input.IsMember = IsMemberEnum.ALL.ToString();
            }
            else
            {
                input.BeginBabyBirthday = item.BeginBabyBirthday;
                input.BeginPointsBalance = item.BeginPointsBalance;
                input.BaySex = item.BaySex;
                input.ChannelID = item.ChannelID;
                input.ChannelName = item.ChannelName;
                input.EndBabyBirthday = item.EndBabyBirthday;
                input.EndPointsBalance = item.EndPointsBalance;
                input.LastBuyProduct = item.LastBuyProduct;
                input.MemberCategory = item.MemberCategory;
                input.OfficialCity = item.OfficialCity;
                input.OrganizeCity = item.OrganizeCity;
                input.TargetID = item.TargetID;
                input.TargetName = item.TargetName;
                input.MotherType = item.MotherType;
                input.IsMember = item.IsMember;
            }
            if (input.IsTask == 0)
            {
                input.ExecTaskTime = DateTime.Now;

            }

            input.SendState = (int)MpMessageTaskState.Wait;

            return await base.Create(input);
        }

        public override async Task<MpMessageDto> Update(MpMessageDto input)
        {
            if (!input.GroupID.HasValue)
                input.GroupID = 0;
            if (input.SendState!= (int)MpMessageTaskState.Wait)
            {
                throw new UserFriendlyException("对不起，您所提交的发送信息已经进入发送队列，不能修改。");
            }
            input.GroupName =await GetGroupName(input.MpID, input.GroupID.Value);
            var item = _mpGroupItemRepository.FirstOrDefault(m => m.IsDeleted == false && m.ParentID == input.GroupID);
            if (item == null)
            {
                input.IsMember = IsMemberEnum.ALL.ToString();
            }
            else
            {
                input.BeginBabyBirthday = item.BeginBabyBirthday;
                input.BeginPointsBalance = item.BeginPointsBalance;
                input.BaySex = item.BaySex;
                input.ChannelID = item.ChannelID;
                input.ChannelName = item.ChannelName;
                input.EndBabyBirthday = item.EndBabyBirthday;
                input.EndPointsBalance = item.EndPointsBalance;
                input.LastBuyProduct = item.LastBuyProduct;
                input.MemberCategory = item.MemberCategory;
                input.OfficialCity = item.OfficialCity;
                input.OrganizeCity = item.OrganizeCity;
                input.TargetID = item.TargetID;
                input.TargetName = item.TargetName;
                input.MotherType = item.MotherType;
                input.IsMember = item.IsMember;
            }
            if (input.IsTask == 0 && input.State != "SEND_SUCCESS")
            {
                input.ExecTaskTime = DateTime.Now;

            }

            return await base.Update(input);
        }

        public async Task<MpMessageDto> TaskUpdate(MpMessageDto input)
        {
            if (!input.GroupID.HasValue)
                input.GroupID = 0;
            input.GroupName = await GetGroupName(input.MpID, input.GroupID.Value);
            var item = _mpGroupItemRepository.FirstOrDefault(m => m.IsDeleted == false && m.ParentID == input.GroupID);
            if (item == null)
            {
                input.IsMember = IsMemberEnum.ALL.ToString();
            }
            else
            {
                input.BeginBabyBirthday = item.BeginBabyBirthday;
                input.BeginPointsBalance = item.BeginPointsBalance;
                input.BaySex = item.BaySex;
                input.ChannelID = item.ChannelID;
                input.ChannelName = item.ChannelName;
                input.EndBabyBirthday = item.EndBabyBirthday;
                input.EndPointsBalance = item.EndPointsBalance;
                input.LastBuyProduct = item.LastBuyProduct;
                input.MemberCategory = item.MemberCategory;
                input.OfficialCity = item.OfficialCity;
                input.OrganizeCity = item.OrganizeCity;
                input.TargetID = item.TargetID;
                input.TargetName = item.TargetName;
                input.MotherType = item.MotherType;
                input.IsMember = item.IsMember;
            }
            if (input.IsTask == 0 && input.State != "SEND_SUCCESS")
            {
                input.ExecTaskTime = DateTime.Now;

            }

            return await base.Update(input);
        }

        public async Task<MpMessageDto> GetModelByMessageTypeAsync(string MessageType, int mpId)
        {
            CheckGetPermission();

            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(!string.IsNullOrWhiteSpace(MessageType), c => c.MessageType == MessageType)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

        public async Task<List<MpMessageDto>> GetNotYetSendContent()
        {
            var state = (int)MpMessageTaskState.Success;
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false
                && m.IsTask == 1
                && m.ExecTaskTime <= DateTime.Now
                && m.SendState == state ))).Select(MapToEntityDto).ToList();
        }

        public async Task UpdateSendState(List<MpMessageDto> inputs)
        {
            foreach (var item in inputs)
            {
                var entity = Repository.Get(item.Id);
                entity.State = item.State;
                entity.FinishDate = item.FinishDate;
                entity.SuccessCount = item.SuccessCount;
                entity.SendState = item.SendState;
                entity.FailCount = item.FailCount;
                entity.SendCount = item.SendCount;
                entity.WxMsgID = item.WxMsgID;
                await Repository.UpdateAsync(entity);
            }
        }

        public async Task<MpMessageDto> GetById(int id)
        {
            return MapToEntityDto(await Repository.GetAsync(id));
        }

        public async Task<MpMessageDto> GetFirstOrDefault(GetMpMessagesInput input)
        {
            var data = await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input));
            var result = ObjectMapper.Map<MpMessageDto>(data);
            return result;
        }

        private MpMessageOtherDataListOutput MapToMpMessageOtherData(MpMessage m) {
            return new MpMessageOtherDataListOutput()
            {
                NameOrContent = m.MessageType == "text" ? m.Content : (
                 m.MessageType == "image" ? m.ImageName : (
                 m.MessageType == "voice" ? m.VoiceName : (
                 m.MessageType == "video" ? m.VideoName : (
                 m.MessageType == "mpnews" ? m.ArticleName : (
                 m.MessageType == "mpmultinews" ? m.ArticleGroupName : ""
                 )
                 )
                 )
                 )
                 ),
                Id = m.Id,
                LastModificationTime = m.LastModificationTime,
                MessageType = m.MessageType,
                ExecTaskTime = m.ExecTaskTime,
                FailCount = m.FailCount,
                FinishDate = m.FinishDate,
                IsTask = m.IsTask,
                SendCount = m.SendCount,
                SendState = m.SendState,
                State = m.State,
                SuccessCount = m.SuccessCount
            };
        }
    }
}
