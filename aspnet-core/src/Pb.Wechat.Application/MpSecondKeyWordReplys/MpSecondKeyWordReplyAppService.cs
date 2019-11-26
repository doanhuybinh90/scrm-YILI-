using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSecondKeyWordReplys.Dto;
using Pb.Wechat.MpSecondKeyWordReplys.Exporting;
using Pb.Wechat.UserMps;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSecondKeyWordReplys
{
    public class MpSecondKeyWordReplyAppService : AsyncCrudAppService<MpSecondKeyWordReply, MpSecondKeyWordReplyDto, int, GetMpSecondKeyWordReplysInput, MpSecondKeyWordReplyDto, MpSecondKeyWordReplyDto>, IMpSecondKeyWordReplyAppService
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpSecondKeyWordReplyListExcelExporter _mpSecondKeyWordReplyListExcelExporter;
        public MpSecondKeyWordReplyAppService(IRepository<MpSecondKeyWordReply, int> repository, IMpSecondKeyWordReplyListExcelExporter mpSecondKeyWordReplyListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _mpSecondKeyWordReplyListExcelExporter = mpSecondKeyWordReplyListExcelExporter;
            _userMpAppService = userMpAppService;
        }

        public override Task<MpSecondKeyWordReplyDto> Create(MpSecondKeyWordReplyDto input)
        {
            var model = Repository.FirstOrDefault(m => m.IsDeleted == false && m.KeyWord == input.KeyWord && input.ParentId == m.ParentId);
            if (model == null)
                return base.Create(input);
            else
            {
                throw new UserFriendlyException("对不起，已经有相同关键字不能重复添加");
            }
        }

        public override Task<MpSecondKeyWordReplyDto> Update(MpSecondKeyWordReplyDto input)
        {
            var model = Repository.Count(m => m.IsDeleted == false && m.KeyWord == input.KeyWord && input.ParentId == m.ParentId && m.Id != input.Id);
            if (model == 0)
                return base.Update(input);
            else
                throw new UserFriendlyException("对不起，已经有相同关键字不能重复添加");
        }

        protected override IQueryable<MpSecondKeyWordReply> CreateFilteredQuery(GetMpSecondKeyWordReplysInput input)
        {
            var msgtype = input.ReplyType == null ? "" : input.ReplyType.ToString();
            return Repository.GetAll()
                .WhereIf(input.ParentId.HasValue, c => c.ParentId == input.ParentId.Value)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.KeyWord.Contains(input.Keyword))
                .WhereIf(input.ReplyType.HasValue, c => c.ReplyType == msgtype);
        }
        public async Task<FileDto> GetListToExcel(GetMpSecondKeyWordReplysInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpSecondKeyWordReplyListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpSecondKeyWordReplyDto> GetEntityByKeyWordAsync(string content, int parentId)
        {
            return MapToEntityDto(await base.Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.KeyWord == content && m.ParentId == parentId));
        }

        public async Task<PagedResultDto<MpSecondKeyWordOutput>> GetKeywordsPage(GetMpSecondKeyWordReplysInput input)
        {
            CheckGetAllPermission();
            var query = CreateFilteredQuery(input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<MpSecondKeyWordOutput>(
                totalCount,
                (await AsyncQueryableExecuter.ToListAsync(query)).Select(m => new MpSecondKeyWordOutput
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
                    LastModificationTime = m.LastModificationTime.HasValue ? m.LastModificationTime : m.CreationTime,
                    ReplyType = m.ReplyType,
                    KeyWord = m.KeyWord,
                    ParentId = m.ParentId
                }).ToList()
                );
        }
    }
}
