using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.MpFans.Dto;
using Pb.Wechat.MpFans.Exporting;
using Pb.Wechat.UserMps;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Abp;
using Pb.Wechat.MpGroups;
using Pb.Wechat.MpFansGroupMaps;
using Abp.Application.Services.Dto;
using System;
using Pb.Wechat.MpFansTagItems;

namespace Pb.Wechat.MpFans
{
    public class MpFanAppService : AsyncCrudAppService<MpFan, MpFanDto, int, GetMpFansInput, MpFanDto, MpFanDto>, IMpFanAppService
    {
        private readonly IMpFanListExcelExporter _MpFanListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IRepository<MpGroup, int> _mpGroupRepository;
        private readonly IRepository<MpFansGroupMap, int> _mpFansGroupMapRepository;
        private readonly IRepository<MpFansTagItem, long> _mpFansTagItemRepository;
        public MpFanAppService(IRepository<MpFan, int> repository, IMpFanListExcelExporter MpFanListExcelExporter, IUserMpAppService userMpAppService, IRepository<MpGroup, int> mpGroupRepository, IRepository<MpFansGroupMap, int> mpFansGroupMapRepository, IRepository<MpFansTagItem, long> mpFansTagItemRepository) : base(repository)
        {
            _MpFanListExcelExporter = MpFanListExcelExporter;
            _userMpAppService = userMpAppService;
            _mpGroupRepository = mpGroupRepository;
            _mpFansGroupMapRepository = mpFansGroupMapRepository;
            _mpFansTagItemRepository = mpFansTagItemRepository;
        }

        protected override IQueryable<MpFan> CreateFilteredQuery(GetMpFansInput input)
        {
            var _channelType = input.ChannelType.ToString();
            List<int?> _channelIds = null;
            if (!string.IsNullOrWhiteSpace(input.ChannelIDs))
            {
                _channelIds = new List<int?>();
                var array = input.ChannelIDs.Split(',');
                foreach (var item in array)
                {
                    _channelIds.Add(int.Parse(item));
                }
            }
            var ismember = -1;
            if (input.IsMember != null)
                ismember = Convert.ToInt32(input.IsMember.Value);
            string[] openIdArray = null;
            if (!string.IsNullOrWhiteSpace(input.OpenIDs))
                openIdArray = input.OpenIDs.Split(',');

            var result = Repository.GetAll()
                .WhereIf(input.MpID.HasValue, c => c.MpID == input.MpID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.OpenID), c => c.OpenID.Contains(input.OpenID))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NickName), c => c.NickName.Contains(input.NickName))
                .WhereIf(input.ChannelID != null && input.ChannelID != 0, c => c.ChannelID == input.ChannelID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ChannelName), c => c.ChannelName.Contains(input.ChannelName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.UnionName), c => c.UnionName.Contains(input.UnionName))
                .WhereIf(input.GroupID != 0, c => c.GroupID == input.GroupID)
                .WhereIf(input.SubscribeStartDate.HasValue, c => c.SubscribeTime >= input.SubscribeStartDate.Value)
                .WhereIf(input.SubscribeEndDate.HasValue, c => c.SubscribeTime <= input.SubscribeEndDate.Value)
                .WhereIf(ismember == 1, c => c.MemberID > 0)
                .WhereIf(ismember == 0, c => c.MemberID == 0)
                .WhereIf(_channelIds != null, c => _channelIds.Contains(c.ChannelID))
                .WhereIf(!string.IsNullOrWhiteSpace(_channelType), c => c.ChannelType.Contains(_channelType))
                .WhereIf(input.IsFans != null, c => c.IsFans == input.IsFans);
            if (input.GroupSearch != null && input.GroupSearch == true)
                result = result.Where(c => openIdArray == null ? c.OpenID == null : openIdArray.Contains(c.OpenID));
            Logger.Info("");
            return result;
        }

        public async Task<FileDto> GetListToExcel(GetMpFansInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpFanListExcelExporter.ExportToFile(dtos);
        }

        public async Task<List<MpFanDto>> FindByNickName(string nickName)
        {
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.NickName == nickName && m.IsDeleted == false))).Select(MapToEntityDto).ToList();
        }



        public async Task<List<NameValue<string>>> GetCountrys()
        {
            var result = new List<NameValue<string>>();
            var datas = (await Repository.GetAllListAsync()).Select(m => new { Id = m.Country, Name = m.Country }).Distinct().ToList();
            foreach (var data in datas)
            {
                var value = new NameValue() { Value = data.Id.ToString(), Name = data.Name };
                result.Add(value);
            }
            return result;
        }

        public async Task<List<NameValue<string>>> GetProvinces(CountryInputDto input)
        {
            var result = new List<NameValue<string>>();
            var datas = (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.Country == input.Country)))
                .Select(m => new { Id = m.Province, Name = m.Province }).Distinct().ToList();
            foreach (var data in datas)
            {
                var value = new NameValue() { Value = data.Id.ToString(), Name = data.Name };
                result.Add(value);
            }
            return result;
        }
        public async Task<List<NameValue<string>>> GetCitys(ProvinceInputDto input)
        {
            var result = new List<NameValue<string>>();
            var datas = (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.Country == input.Country && m.Province == input.Province)))
                .Select(m => new { Id = m.City, Name = m.City }).Distinct().ToList();
            foreach (var data in datas)
            {
                var value = new NameValue() { Value = data.Id.ToString(), Name = data.Name };
                result.Add(value);
            }
            return result;
        }

        public async Task<List<MpFanDto>> GetAllByMpId(int mpId)
        {
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.IsDeleted == false && m.MpID == mpId))).Select(MapToEntityDto).ToList();
        }

        public async Task<List<MpFanDto>> GetAllByMpIdAndGroupId(int mpId, List<int> groupIds)
        {
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.MpID == mpId && m.IsDeleted == false && groupIds.Contains(m.GroupID ?? -1)))).Select(MapToEntityDto).ToList();
        }

        public async Task<MpFanDto> GetFirstOrDefault(GetMpFansInput input)
        {
            var data = await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input));
            return MapToEntityDto(data);
        }
        /// <summary>
        /// 追加分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateFansGroup(MpFanGroupDto input)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var existFansIds = (await _mpFansGroupMapRepository.GetAllListAsync(m => m.IsDeleted == false && m.MpID == mpid && input.FansIds.Contains(m.FansID) && m.GroupID == input.GroupID)).Select(m => m.FansID).ToList();
            var fansIds = input.FansIds.Except(existFansIds);
            foreach (var item in fansIds)
            {
                await _mpFansGroupMapRepository.InsertAsync(new MpFansGroupMap
                {
                    MpID = mpid,
                    FansID = item,
                    GroupID = input.GroupID,
                    GroupName = input.GroupName
                });
            }
            await RefreshFansGroup(input.GroupID);
        }
        /// <summary>
        /// 重置分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task RenewFansGroup(MpFanGroupDto input)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var renewGroupIds = _mpFansGroupMapRepository.GetAll().Where(m => m.IsDeleted == false && input.FansIds.Contains(m.FansID)).Select(m => m.GroupID).Distinct().ToList();
            await _mpFansGroupMapRepository.DeleteAsync(m => m.IsDeleted == false && input.FansIds.Contains(m.FansID));
            foreach (var item in input.FansIds)
            {
                await _mpFansGroupMapRepository.InsertAsync(new MpFansGroupMap
                {
                    MpID = mpid,
                    FansID = item,
                    GroupID = input.GroupID,
                    GroupName = input.GroupName
                });
            }
            if (!renewGroupIds.Contains(input.GroupID))
                renewGroupIds.Add(input.GroupID);
            foreach (var gpId in renewGroupIds)
            {
                await RefreshFansGroup(gpId);
            }

        }

        public async Task RefreshFansGroup(int groupId)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var groupCount = await _mpFansGroupMapRepository.CountAsync(m => m.IsDeleted == false && m.GroupID == groupId);
            _mpGroupRepository.Update(groupId, m =>
            {
                m.FansCount = groupCount;
            });
        }

        public override async Task<PagedResultDto<MpFanDto>> GetAll(GetMpFansInput input)
        {

            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var fansIds = entities.Select(m => m.Id).ToList();
            var fansGroupMaps = await _mpFansGroupMapRepository.GetAllListAsync(m => m.IsDeleted == false && fansIds.Contains(m.FansID));
            if (fansGroupMaps != null && fansGroupMaps.Count > 0)
            {
                foreach (var entity in entities)
                {
                    entity.GroupName = string.Join(",", fansGroupMaps.Select(m => m.GroupName).Distinct());
                }
            }

            return new PagedResultDto<MpFanDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );


        }

        public List<string> FilterDatas(int mpId, List<string> openIds = null)
        {
            if (openIds != null)
                return Repository.GetAll().Where(m => m.IsDeleted == false && openIds.Contains(m.OpenID)).Select(m => m.OpenID).ToList();
            else
                return Repository.GetAll().Where(m => m.IsDeleted == false).Select(m => m.OpenID).ToList();
        }

        public async Task<long> FilterCountAsync(int mpId)
        {
            return await Repository.LongCountAsync(m => m.IsDeleted == false && m.MpID == mpId);
        }
        public List<string> FilterNotMemberDatas(int mpId)
        {
            return Repository.GetAll().Where(m => m.IsDeleted == false && m.MemberID == 0).Select(m => m.OpenID).ToList();
        }
        public List<string> FilterTagDatas(int mpId,string tagIds)
        {
            var tagids = new List<int>();
            var tagidstr = tagIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
            int tagid = -1;
            foreach (var item in tagidstr) {
                if (int.TryParse(item, out tagid))
                    tagids.Add(tagid);
            }
            if (tagids.Count == 0)
                tagids.Add(-1);
            var openids = from a in Repository.GetAll().Where(m => m.IsDeleted == false)
                          join b in _mpFansTagItemRepository.GetAll().Where(c => tagids.Contains(c.TagId))
                          on a.Id equals b.FansId
                          select a.OpenID;
            return openids.ToList();
        }

        public async Task<MpFanDto> GetFirstOrDefaultByOpenID(string openId)
        {
            return ObjectMapper.Map<MpFanDto>(await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.OpenID == openId));
        }
    }
}
