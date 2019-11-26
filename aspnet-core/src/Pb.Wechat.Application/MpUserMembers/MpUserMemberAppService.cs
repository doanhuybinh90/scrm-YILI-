using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpUserMembers.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpUserMembers
{
    //[AbpAuthorize(AppPermissions.Pages_MpUserMembers)]
    public class MpUserMemberAppService : AsyncCrudAppService<MpUserMember, MpUserMemberDto, int, GetMpUserMembersInput, MpUserMemberDto, MpUserMemberDto>, IMpUserMemberAppService
    {

        private readonly ICacheManager _cacheManager;
        private readonly IMpUserMemberListExcelExporter _MpUserMemberListExcelExporter;
        private readonly IRepository<MpFan, int> _fansRepository;
        public MpUserMemberAppService(IRepository<MpUserMember, int> repository, IMpUserMemberListExcelExporter MpUserMemberListExcelExporter, ICacheManager cacheManager, IRepository<MpFan, int> fansRepository) : base(repository)
        {
            _MpUserMemberListExcelExporter = MpUserMemberListExcelExporter;
            _fansRepository = fansRepository;
            _cacheManager = cacheManager;
        }

        protected override IQueryable<MpUserMember> CreateFilteredQuery(GetMpUserMembersInput input)
        {
            string[] openIdArray = null;
            if (!string.IsNullOrWhiteSpace(input.OpenIDs))
                openIdArray = input.OpenIDs.Split(',');

            var result = Repository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.OpenID), c => c.OpenID.Contains(input.OpenID))
                .WhereIf(!string.IsNullOrWhiteSpace(input.MobilePhone), c => c.MobilePhone == input.MobilePhone)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MemberPassword), c => c.MemberPassword==input.MemberPassword)
                .WhereIf(input.ChannelID!=null,c=>c.ChannelID==input.ChannelID)
                .WhereIf(input.IsBinding!=null,c=>c.IsBinding==input.IsBinding)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MemberUserName),c=>c.MemeberUserName.Contains(input.MemberUserName))
                .WhereIf(input.CreationStartTime!=null,c=>c.CreationTime>=input.CreationStartTime)
                .WhereIf(input.CreationEndTime != null, c => c.CreationTime <= input.CreationEndTime)
                ;
            if (input.GroupSearch != null && input.GroupSearch == true)
                result = result.Where(c => openIdArray == null ? c.OpenID == null : openIdArray.Contains(c.OpenID));
            return result;
        }

        public async override Task<MpUserMemberDto> Create(MpUserMemberDto input)
        {
            var result = await base.Create(input);
            await _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey).SetAsync(input.OpenID, result);
            return result;
        }

        public async override Task<MpUserMemberDto> Update(MpUserMemberDto input)
        {
            var result = await base.Update(input);
            await _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey).SetAsync(input.OpenID, result);
            return result;
        }

        public async Task<FileDto> GetListToExcel(GetMpUserMembersInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpUserMemberListExcelExporter.ExportToFile(dtos);
        }


        public async Task<MpUserMemberDto> GetByOpenID(string openId, bool renew = false)
        {
            if (!renew)
            {
                var mpusermembercache = _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey);
                return await mpusermembercache.GetAsync<string, MpUserMemberDto>(openId, async key =>
                {
                    return MapToEntityDto(await Repository.FirstOrDefaultAsync(m => m.OpenID == openId && m.IsDeleted == false && m.IsBinding == true));
                });
            }
           else
            {
                var result = MapToEntityDto(await Repository.FirstOrDefaultAsync(m => m.OpenID == openId && m.IsDeleted == false && m.IsBinding == true));
                await _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey).SetAsync(openId,result);
                return result;
            }
        }
        public async Task<string> GetMgccAuthKey(string openId)
        {
            var authkeycache = _cacheManager.GetCache(AppConsts.Cache_OpenID2AuthKey);
            return await authkeycache.GetAsync<string, string>(openId, async key =>
              {
                  var mgccAuthKey = Guid.NewGuid().ToString().Replace("-", "");
                  var openidcache = _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID);
                  await openidcache.SetAsync(mgccAuthKey, openId);
                  var memberInfo =await Repository.FirstOrDefaultAsync(c => c.OpenID == openId && c.IsDeleted == false && c.IsBinding == true);
                  if (memberInfo != null)
                  {
                      memberInfo.MgccAuthkey = mgccAuthKey;
                      Repository.Update(memberInfo);
                      var mpusermembercache = _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey);
                      var meminfoDto = MapToEntityDto(memberInfo);
                      await mpusermembercache.SetAsync(openId, meminfoDto);
                  }

                  return mgccAuthKey;
              });
        }

        public async Task<List<string>> FilterOpenIds(string motherType, List<string> openIds)
        {
            var result =Repository.GetAll().Where(m => openIds.Contains(m.OpenID) && m.IsDeleted == false && m.IsBinding == true);
            if (motherType == MpGroups.Dto.MotherType.UnPregnant.ToString())
            {
                DateTime date = DateTime.Now.AddDays(280);
                result = result.Where(m => m.BabyBirthday == null || m.BabyBirthday >= date);
            }
            else if (motherType == MpGroups.Dto.MotherType.Pregnant.ToString())
            {
                DateTime date = DateTime.Now.AddDays(280);
                result = result.Where(m => m.BabyBirthday == null || (m.BabyBirthday <= date && m.BabyBirthday >= DateTime.Now));
            }
            return await AsyncQueryableExecuter.ToListAsync(result.Select(m=>m.OpenID));
        }

        public async Task DeleteOtherSame(string openId, int id)
        {
            await Repository.DeleteAsync(m => m.OpenID == openId && m.Id != id);
        }
        public async Task<List<MpUserMemberDto>> GetList(GetMpUserMembersInput input)
        {
            var list=CreateFilteredQuery(input).ToList();
            return ObjectMapper.Map<List<MpUserMemberDto>>(list);
        }
        /// <summary>
        /// 解绑会员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MpUserMemberDto> UnBinding(UnBindindInput input)
        {
            try
            {
                var data = await base.Get(new EntityDto<int> { Id = input.Id });
                var fan = await _fansRepository.FirstOrDefaultAsync(m => m.OpenID == data.OpenID && m.IsDeleted == false);
                fan.MemberID = 0;
                await _fansRepository.UpdateAsync(fan);
                data.IsBinding = false;
                data.UnBindingReason = input.UnBindingReason;
                await _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey).RemoveAsync(data.OpenID);
                var authkey =await _cacheManager.GetCache(AppConsts.Cache_OpenID2AuthKey).GetOrDefaultAsync(data.OpenID);
                if (authkey != null) {
                    var authkeyStr = authkey.ToString();
                    await _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID).RemoveAsync(authkeyStr);
                    await _cacheManager.GetCache(AppConsts.Cache_OpenID2AuthKey).RemoveAsync(data.OpenID);
                }
                return await base.Update(data);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException($"解绑失败，原因{ex.Message}");
            }
           
        }
        public async Task<MpUserMemberDto> GetByMobile(string mobile)
        {
            return ObjectMapper.Map<MpUserMemberDto>(await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.MobilePhone == mobile && m.IsBinding == true));
        }
    }
}
