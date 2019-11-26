using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpChannels.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpChannels
{
    //[AbpAuthorize(AppPermissions.Pages_MpChannels)]
    public class MpChannelAppService : AsyncCrudAppService<MpChannel, MpChannelDto, int, GetMpChannelsInput, MpChannelDto, MpChannelDto>, IMpChannelAppService
    {
        private readonly IWxMediaAppService _wxMediaAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpChannelListExcelExporter _mpChannelListExcelExporter;
        private readonly IRepository<MpUserMember, int> _userMemberRepository;
        private readonly IRepository<MpFan, int> _fanRepository;
        private readonly ICacheManager _cacheManager;
        public MpChannelAppService(IRepository<MpChannel, int> repository, IMpChannelListExcelExporter mpChannelListExcelExporter, IUserMpAppService userMpAppService, IWxMediaAppService wxMediaAppService, IRepository<MpUserMember, int> userMemberRepository, IRepository<MpFan, int> fanRepository, ICacheManager cacheManager) : base(repository)
        {
            _mpChannelListExcelExporter = mpChannelListExcelExporter;
            _userMpAppService = userMpAppService;
            _wxMediaAppService = wxMediaAppService;
            _userMemberRepository = userMemberRepository;
            _fanRepository = fanRepository;
            _cacheManager = cacheManager;
        }

        protected override IQueryable<MpChannel> CreateFilteredQuery(GetMpChannelsInput input)
        {
            var channelType = input.ChannelType != null ? input.ChannelType.ToString() : null;
            var tempType = ChannelType.QR_STR_SCENE.ToString();
            var limitType = ChannelType.QR_LIMIT_STR_SCENE.ToString();
            var nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .Where(c=>c.EndTime>= nowDate || c.EndTime==null)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Title), c => c.Name.Contains(input.Title))
                .WhereIf(!string.IsNullOrWhiteSpace(input.EventKey), c => c.EventKey == input.EventKey)
                .WhereIf(!string.IsNullOrWhiteSpace(channelType), c => c.ChannelType == channelType)
                .WhereIf(input.CreationStartTime != null, c => c.CreationTime >= input.CreationStartTime)
                .WhereIf(input.CreationEndTime != null, c => c.CreationTime <= input.CreationEndTime)
                .WhereIf(input.ValidityStartTime != null, c => (c.EndTime >= input.ValidityStartTime && c.ChannelType==tempType)|| c.ChannelType==limitType)
                .WhereIf(input.ValidityEndTime != null, c => (c.EndTime <= input.ValidityEndTime && c.ChannelType == tempType) || c.ChannelType == limitType)
                ;
        }

        public override async Task<MpChannelDto> Update(MpChannelDto input)
        {
            if (input.IsMcChannel == 0)
            {
                if (!input.EventKey.StartsWith("qrcode."))
                    input.EventKey = "qrcode." + input.EventKey;
            }
            var isHave = await Repository.CountAsync(m => m.IsDeleted == false && m.EventKey == input.EventKey && m.Id!=input.Id);
            if (isHave > 0)
                throw new UserFriendlyException("对不起，您提交的二维码已存在，请检查。");
            var expireSeconds = 0;
            QrCodeResult data = null;
            if (input.ChannelType == ChannelType.QR_STR_SCENE.ToString())
            {
                if (input.ValidityDay <= 0)
                    throw new UserFriendlyException("临时二维码，请填写过期天数。");
                if (input.ValidityDay > 0)
                {
                    expireSeconds = input.ValidityDay * 24 * 60 * 60;
                    input.EndTime = Convert.ToDateTime(input.StartTime).AddDays(input.ValidityDay);
                }
            }

            try
            {
                data = await _wxMediaAppService.SaveQrCode(input.MpID, input.Name, input.EventKey, input.ChannelType, expireSeconds);
                input.Ticket = data.Ticket;
                input.FilePath = data.FilePath;
                input.FileUrl = data.Url;

                await _cacheManager.GetCache(AppConsts.Cache_CallBack).ClearAsync();
            }
            catch (System.Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }

            return await base.Update(input);
        }
        public override async Task<MpChannelDto> Create(MpChannelDto input)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            input.MpID = mpid;
            if (input.IsMcChannel == 0)
            {
                if (!input.EventKey.StartsWith("qrcode."))
                    input.EventKey = "qrcode." + input.EventKey;
            }
            var isHave = await Repository.CountAsync(m => m.IsDeleted == false && m.EventKey == input.EventKey);
            if (isHave>0)
                throw new UserFriendlyException("对不起，您提交的二维码已存在，请检查。");
            var expireSeconds = 0;
            QrCodeResult data = null;
            if (input.ChannelType == ChannelType.QR_STR_SCENE.ToString())
            {
                if (input.ValidityDay <= 0)
                    throw new UserFriendlyException("临时二维码，请填写过期天数。");
                if (input.ValidityDay > 0)
                {
                    expireSeconds = input.ValidityDay * 24 * 60 * 60;
                    input.EndTime = DateTime.Now.AddDays(input.ValidityDay);
                    input.StartTime = DateTime.Now;
                }
            }

            try
            {
                data = await _wxMediaAppService.SaveQrCode(input.MpID, input.Name, input.EventKey, input.ChannelType, expireSeconds);
                input.Ticket = data.Ticket;
                input.FilePath = data.FilePath;
                input.FileUrl = data.Url;
            }
            catch (System.Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }

            CheckCreatePermission();

            var entity = MapToEntity(input);
            Repository.Insert(entity);

            return MapToEntityDto(entity);
        }
        public async Task<FileDto> GetListToExcel(GetMpChannelsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpChannelListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpChannelDto> GetFirstOrDefault(GetMpChannelsInput input)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input)));
        }

        public async Task<List<NameValue<string>>> GetChannels()
        {
            //string searchTerm
            //.Where(m => m.Name.ToLower().Contains(searchTerm.ToLower()))
            var result = new List<NameValue<string>>();
            var datas = Repository.GetAllList().Select(m => new { Id = m.Id, Name = m.Name }).ToList();
            foreach (var data in datas)
            {
                var value = new NameValue() { Value = data.Id.ToString(), Name = data.Name };
                result.Add(value);
            }
            return result;
        }

        public async Task<PagedResultDto<MpChannelOutput>> GetChannelPage(GetMpChannelsInput input)
        {
            CheckGetAllPermission();
            var query = CreateFilteredQuery(input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<MpChannelOutput>(
                totalCount,
                (await AsyncQueryableExecuter.ToListAsync(query)).Select(m => new MpChannelOutput
                {
                    ContentOrName = m.ReplyType == "text" ? m.Content : (
                m.ReplyType == "image" ? m.ImageName : (
                m.ReplyType == "voice" ? m.VoiceName : (
                m.ReplyType == "video" ? m.VideoName : (
                m.ReplyType == "mpnews" ? m.ArticleName : (
                m.ReplyType == "mpmultinews" ? m.ArticleGroupName : ""
                )
                )
                )
                )
                ),
                    Id = m.Id,
                    LastModificationTime = m.LastModificationTime,
                    ReplyType = m.ReplyType,
                    ChannelType = m.ChannelType,
                    Code = m.Code,
                    ChannelName=m.ChannelName,
                    EventKey = m.EventKey,
                    FilePath = m.FilePath,
                    FileUrl = m.FileUrl,
                    Name = m.Name,
                    CreationTime=m.CreationTime,
                    EndTime=m.EndTime,
                    TagIds=m.TagIds,
                    TagNames=m.TagNames
                }).ToList()
                );
        }

        public async Task<List<MpChannelDto>> GetAllChannel(GetMpChannelsInput input)
        {
            return (await AsyncQueryableExecuter.ToListAsync(CreateFilteredQuery(input))).Select(MapToEntityDto).ToList();
        }

        public async Task ClearRegister()
        {

            await _userMemberRepository.DeleteAsync(m => m.IsDeleted == false);
            var allFans = await _fanRepository.GetAllListAsync(m => m.MemberID > 0);
            foreach (var item in allFans)
            {
                await _fanRepository.UpdateAsync(item.Id, async m =>
                {
                    m.MemberID = 0;
                    await _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey).RemoveAsync(m.OpenID);
                });
            }
        }
    }
}
