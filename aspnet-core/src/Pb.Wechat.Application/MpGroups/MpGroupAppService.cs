using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.MpGroups.Dto;
using Pb.Wechat.MpGroups.Exporting;
using Pb.Wechat.UserMps;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Pb.Wechat.MpFans;
using Abp.UI;
using Pb.Wechat.MpFansGroupMaps;
using Abp;
using Pb.Wechat.MpGroupItems;
using Pb.Wechat.YiliLastBuyProducts;
using Pb.Wechat.YiliOfficialCitys;
using Pb.Wechat.YiliOrganizeCitys;
using Pb.Wechat.YiliMemberTypes;
using Pb.Wechat.MpMessages;
using Pb.Wechat.MpMessages.Dto;

namespace Pb.Wechat.MpGroups
{
    public class MpGroupAppService : AsyncCrudAppService<MpGroup, MpGroupDto, int, GetMpGroupsInput, MpGroupDto, MpGroupDto>, IMpGroupAppService
    {
        private readonly IMpGroupListExcelExporter _MpGroupListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IRepository<MpFan, int> _mpFanrepository;
        private readonly IRepository<MpFansGroupMap, int> _mpFansGroupMapRepository;
        private readonly IRepository<MpGroupItem, int> _mpGroupItemRepository;
        private readonly IRepository<MpMessage, int> _mpMessageRepository;
        private readonly IRepository<YiliLastBuyProduct, int> _productRepository;
        private readonly IRepository<YiliOfficialCity, int> _officialCityRepository;
        private readonly IRepository<YiliOrganizeCity, int> _organizeCityRepository;
        private readonly IRepository<YiliMemberType, int> _memberTypeRepository;
        public MpGroupAppService(IRepository<MpGroup, int> repository, IMpGroupListExcelExporter MpGroupListExcelExporter, IUserMpAppService userMpAppService, IRepository<MpFan, int> mpFanrepository, IRepository<MpFansGroupMap, int> mpFansGroupMapRepository, IRepository<MpGroupItem, int> mpGroupItemRepository, IRepository<YiliLastBuyProduct, int> productRepository, IRepository<YiliOfficialCity, int> officialCityRepository, IRepository<YiliOrganizeCity, int> organizeCityRepository, IRepository<YiliMemberType, int> memberTypeRepository, IRepository<MpMessage, int> mpMessageRepository) : base(repository)
        {
            _MpGroupListExcelExporter = MpGroupListExcelExporter;
            _userMpAppService = userMpAppService;
            _mpFanrepository = mpFanrepository;
            _mpFansGroupMapRepository = mpFansGroupMapRepository;
            _mpGroupItemRepository = mpGroupItemRepository;
            _productRepository = productRepository;
            _officialCityRepository = officialCityRepository;
            _organizeCityRepository = organizeCityRepository;
            _memberTypeRepository = memberTypeRepository;
            _mpMessageRepository = mpMessageRepository;
        }

        protected override IQueryable<MpGroup> CreateFilteredQuery(GetMpGroupsInput input)
        {

            return Repository.GetAll()
                .WhereIf(input.MpID != 0, c => c.MpID == input.MpID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.Name.Contains(input.Name));
        }

        public override async Task<MpGroupDto> Create(MpGroupDto input)
        {

            var sameNameCount = Repository.Count(m => m.IsDeleted == false && m.Name == input.Name);
            if (sameNameCount > 0)
                throw new UserFriendlyException("对不起不能创建相同名称的分组。");
            var maxId = Repository.Query(m => m.Max(t => t.Id));
            if (Repository.Query(m => m.Any(t => t.Id == input.ParentID)))
            {
                var parentModel = Repository.Get(input.ParentID);
                var parentFullPath = parentModel.FullPath;
                var yfullpath = input.FullPath;
                input.FullPath = parentFullPath + "." + (maxId + 1).ToString();
                input.ParentName = parentModel.Name;
                var leafs = Repository.GetAllList(m => m.FullPath.Contains(yfullpath)).ToList();
                foreach (var leaf in leafs)
                {
                    Repository.Update(leaf.Id, m =>
                    {
                        m.FullPath = parentFullPath + "." + m.FullPath;
                        m.Length = parentModel.Length + 1;
                    });
                }
            }
            else
            {
                input.FullPath = (maxId + 1).ToString();
                input.Length = 1;
            }
            input.MpID = await _userMpAppService.GetDefaultMpId();

            input.CreationTime = DateTime.Now;
            input.LastModificationTime = DateTime.Now;
            return await base.Create(input);
        }
        public override Task<MpGroupDto> Update(MpGroupDto input)
        {
            var sameNameCount = Repository.Count(m => m.IsDeleted == false && m.Name == input.Name && m.Id != input.Id);
            if (sameNameCount > 0)
                throw new UserFriendlyException("对不起不能修改已有相同名称的分组。");
            var maxId = Repository.Query(m => m.Max(t => t.Id));
            if (Repository.Query(m => m.Any(t => t.Id == input.ParentID)))
            {
                var parentModel = Repository.Get(input.ParentID);
                var parentFullPath = parentModel.FullPath;
                var yfullpath = input.FullPath;
                input.FullPath = parentFullPath + "." + (maxId + 1).ToString();
                input.ParentName = parentModel.Name;
                var leafs = Repository.GetAllList(m => m.FullPath.Contains(yfullpath)).ToList();
                foreach (var leaf in leafs)
                {
                    Repository.Update(leaf.Id, m => { m.FullPath = parentFullPath + "." + m.FullPath; });
                }
            }
            else
            {
                input.FullPath = (maxId + 1).ToString();
            }
            return base.Update(input);
        }
        public override Task Delete(EntityDto<int> input)
        {
            var _id = input.Id;

            if (Repository.Query(m => m.Any(t => t.IsDeleted == false && t.ParentID == input.Id)))
                throw new UserFriendlyException("对不起，此分组下有其他分组，不能删除。");
            var unfinishstate = new List<int>() { (int)MpMessageTaskState.Wait, (int)MpMessageTaskState.Doing };
            var message = _mpMessageRepository.FirstOrDefault(m => m.GroupID == _id && m.IsDeleted == false && unfinishstate.Contains(m.SendState));
            if (message != null)
                throw new UserFriendlyException("对不起，此分组有群发任务挂载，不能删除。");
            _mpGroupItemRepository.Delete(m => m.ParentID == input.Id);
            return base.Delete(input);
        }

        public async Task<FileDto> GetListToExcel(GetMpGroupsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpGroupListExcelExporter.ExportToFile(dtos);
        }

        public async Task<List<MpGroupDto>> GetList()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.IsDeleted == false && m.MpID == mpid))).Select(MapToEntityDto).ToList();
        }

        public async Task Save(MpGroupDto input)
        {
            await this.Update(input);
        }

        public async Task<ListResultDto<MpGroupDto>> GetGroupUnits()
        {
            return new ListResultDto<MpGroupDto>(
               (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll()))
               .Select(MapToEntityDto).ToList());
        }

        public async Task<List<MpGroupDto>> GetListByIds(int mpid, List<int> ids)
        {
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.IsDeleted == false && m.MpID == mpid && ids.Contains(m.Id)))).Select(MapToEntityDto).ToList(); ;
        }

        public async Task<ListResultDto<MpGroupDto>> GetMpGroupTrees()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var query =
                from ou in Repository.GetAll()
                where ou.IsDeleted == false && ou.MpID == mpid
                select new { ou };

            var items = await query.ToListAsync();

            return new ListResultDto<MpGroupDto>(
                items.Select(item =>
                {
                    var dto = ObjectMapper.Map<MpGroupDto>(item.ou);
                    return dto;
                }).ToList());
        }

        public async Task<MpGroupDto> MoveGroupParent(GetMpGroupsInput input)
        {
            var model = await base.Get(new EntityDto<int> { Id = input.Id });

            //model.FullPath = model.FullPath.Replace(oldParentId, input.NewParentId);
            model.ParentID = input.NewParentId;
            if (input.NewParentId != 0)
            {
                var parentModel = await base.Get(new EntityDto<int> { Id = input.NewParentId });
                model.Length = parentModel.Length + 1;
                model.FullPath = parentModel.FullPath + "." + input.Id;
                model.ParentName = parentModel.Name;
            }
            else
            {
                model.Length = 1;
                model.FullPath = input.Id.ToString();
            }

            return await base.Update(model);
        }
        public async Task<List<NameValue<string>>> GetFirstLevelGroup(GetFirstLevelGroupInput input)
        {
            var result = new List<NameValue<string>>();
            var datas = (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.MpID == input.MpID && m.IsDeleted == false && m.ParentID == 0)))
                .Select(m => new { m.Id, m.Name }).Distinct().ToList();
            foreach (var data in datas)
            {
                var value = new NameValue() { Value = data.Id.ToString(), Name = data.Name };
                result.Add(value);
            }
            return result;
        }

        public async Task<List<NameValue<string>>> GetSecondLevelGroup(GetSecondLevelGroupInput input)
        {
            var result = new List<NameValue<string>>();
            var datas = (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.MpID == input.MpID && m.IsDeleted == false && m.ParentID == input.ParentID)))
                .Select(m => new { m.Id, m.Name }).Distinct().ToList();
            foreach (var data in datas)
            {
                var value = new NameValue() { Value = data.Id.ToString(), Name = data.Name };
                result.Add(value);
            }
            return result;
        }

        public async Task<List<MpGroupDto>> GetListByParentId(int mpId, int parentId)
        {
            return (await Repository.GetAllListAsync(m => m.IsDeleted == false && m.MpID == mpId && (m.ParentID == parentId || m.Id == parentId))).Select(MapToEntityDto).ToList();
        }

        public async Task<MpGroupItemDto> GetGroupItemFirstOrDefault(GetGroupItemInput input)
        {
            return ObjectMapper.Map<MpGroupItemDto>(await _mpGroupItemRepository.GetAll()
                .Where(c => c.Id == input.Id)
                .WhereIf(input.MpID != 0, c => c.MpID == input.MpID)
                .FirstOrDefaultAsync());
        }

        public async Task<MpGroupItemDto> GetItem(int parentId)
        {
            var data = await _mpGroupItemRepository.FirstOrDefaultAsync(m => m.ParentID == parentId);
            var result = ObjectMapper.Map<MpGroupItemDto>(data);
            return result;
        }

        public async Task<List<YiliOrganizeCity>> GetOrganizeCitys()
        {
            return await _organizeCityRepository.GetAllListAsync();
        }
        public async Task<List<YiliOfficialCity>> GetOfficialCitys()
        {
            return await _officialCityRepository.GetAllListAsync();
        }
        public async Task<List<YiliLastBuyProduct>> GetLastBuyProducts()
        {
            return await _productRepository.GetAllListAsync();
        }

        public async Task<List<YiliMemberType>> GetMemberTypes()
        {
            return await _memberTypeRepository.GetAllListAsync();
        }
        public async Task SaveItem(MpGroupItemDto input)
        {
            if (input.MotherType == MotherType.ALL.ToString())
            {
                input.BeginBabyBirthday = new DateTime(1900, 1, 1);
                input.EndBabyBirthday = new DateTime(1900, 1, 1);
            }
            else if (input.MotherType == MotherType.UnPregnant.ToString())
            {
                DateTime date = DateTime.Now.AddDays(280);
                input.BeginBabyBirthday = date;
                input.EndBabyBirthday = new DateTime(1900, 1, 1);
            }
            else if (input.MotherType == MotherType.Pregnant.ToString())
            {
                DateTime date = DateTime.Now.AddDays(280);
                input.BeginBabyBirthday = DateTime.Now;
                input.EndBabyBirthday = date;
            }
            else if (input.MotherType == MotherType.One.ToString())
            {
                DateTime date = DateTime.Now.AddDays(-180);
                input.BeginBabyBirthday = date;
                input.EndBabyBirthday = DateTime.Now;
            }
            else if (input.MotherType == MotherType.Two.ToString())
            {
                DateTime date = DateTime.Now.AddDays(-365);
                DateTime date2 = DateTime.Now.AddDays(-180);
                input.BeginBabyBirthday = date;
                input.EndBabyBirthday = date2;
            }
            else if (input.MotherType == MotherType.Three.ToString())
            {
                DateTime date = DateTime.Now.AddDays(-730);
                DateTime date2 = DateTime.Now.AddDays(-365);
                input.BeginBabyBirthday = date;
                input.EndBabyBirthday = date2;
            }
            else if (input.MotherType == MotherType.Four.ToString())
            {
                DateTime date = DateTime.Now.AddDays(-730);
                input.BeginBabyBirthday = new DateTime(1900, 1, 1);
                input.EndBabyBirthday = date;
            }

            await _mpGroupItemRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<MpGroupItem>(input));
        }
    }
}
