using Abp.Application.Services;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Pb.Wechat.Dto;
using Pb.Wechat.MpKeyWordReplys.Dto;
using Pb.Wechat.MpKeyWordReplys.Exporting;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.UserMps;
using Abp.Application.Services.Dto;
using System;
using Abp.UI;
using Abp.Runtime.Caching;

namespace Pb.Wechat.MpKeyWordReplys
{
    //[AbpAuthorize(AppPermissions.Pages_MpKeyWordReplys)]
    public class MpKeyWordReplyAppService : AsyncCrudAppService<MpKeyWordReply, MpKeyWordReplyDto, int, GetMpKeyWordReplysInput, MpKeyWordReplyDto, MpKeyWordReplyDto>, IMpKeyWordReplyAppService
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpKeyWordReplyListExcelExporter _mpAccountListExcelExporter;
        private readonly ICacheManager _cacheManager;
        //private readonly IRepository<MpMediaImage, int> imageRepository;
        //private readonly IRepository<MpMediaVideo, int> videoRepository;
        //private readonly IRepository<MpMediaVoice, int> voiceRepository;
        //private readonly IRepository<MpMediaArticle, int> artRepository;
        //private readonly IRepository<MpMediaArticleGroup, int> artGroupRepository;
        public MpKeyWordReplyAppService(IRepository<MpKeyWordReply, int> repository, IMpKeyWordReplyListExcelExporter mpAccountListExcelExporter, IUserMpAppService userMpAppService, ICacheManager cacheManager) : base(repository)
        {
            _mpAccountListExcelExporter = mpAccountListExcelExporter;
            _userMpAppService = userMpAppService;
            _cacheManager = cacheManager;
        }

        protected override IQueryable<MpKeyWordReply> CreateFilteredQuery(GetMpKeyWordReplysInput input)
        {
            //var mpid = _userMpAppService.GetDefaultMpId().Result;
          
            var msgtype = input.ReplyType == null ? "" : input.ReplyType.ToString();
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(input.MpID != 0, c => c.MpID == input.MpID)
                .WhereIf(input.ReplyType != null, c => c.ReplyType == msgtype);
        }
        public override Task<MpKeyWordReplyDto> Update(MpKeyWordReplyDto input)
        {
            var count = Repository.Count(m => m.IsDeleted == false && m.KeyWord == input.KeyWord && m.Id!=input.Id);
            if (count > 0)
                throw new UserFriendlyException("对不起，关键词不能已经存在，不能添加相同的关键词回复信息。");
            _cacheManager.GetCache(AppConsts.Cache_CallBack).ClearAsync();
            return base.Update(input);
        }

        public override Task<MpKeyWordReplyDto> Create(MpKeyWordReplyDto input)
        {
            var count = Repository.Count(m => m.IsDeleted == false && m.KeyWord == input.KeyWord);
            if (count>0)
                throw new UserFriendlyException("对不起，关键词不能已经存在，不能添加相同的关键词回复信息。");
            input.LastModificationTime = DateTime.Now;
            return base.Create(input);
        }
        public async Task<FileDto> GetListToExcel(GetMpKeyWordReplysInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpAccountListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpKeyWordReplyDto> GetModelByReplyTypeAsync(string replyType, int mpId)
        {
            CheckGetPermission();
            
            var entity =await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(!string.IsNullOrWhiteSpace(replyType), c => c.ReplyType == replyType)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }
        
        public async Task<MpKeyWordReplyDto> GetEntityByKeyWordAsync(string content, int mpId)
        {
            return MapToEntityDto(await base.Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.KeyWord == content && m.MpID == mpId));
        }

        public async Task<PagedResultDto<MpKeyWordOutput>> GetKeywordsPage(GetMpKeyWordReplysInput input)
        {
            CheckGetAllPermission();
            var query = CreateFilteredQuery(input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<MpKeyWordOutput>(
                totalCount,
                (await AsyncQueryableExecuter.ToListAsync(query)).Select(m => new MpKeyWordOutput
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
                    KeyWord=m.KeyWord
                }).ToList()
                );
        }
    }
}
