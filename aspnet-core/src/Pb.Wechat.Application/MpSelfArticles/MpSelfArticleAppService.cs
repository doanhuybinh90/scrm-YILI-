using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSelfArticles.Exporting;
using Pb.Wechat.UserMps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSelfArticles.Dto.MpSelfArticles
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class MpSelfArticleAppService : AsyncCrudAppService<MpSelfArticle, MpSelfArticleDto, int, GetMpSelfArticlesInput, MpSelfArticleDto, MpSelfArticleDto>, IMpSelfArticleAppService
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpSelfArticleListExcelExporter _MpSelfArticleListExcelExporter;
        public MpSelfArticleAppService(IRepository<MpSelfArticle, int> repository, IMpSelfArticleListExcelExporter MpSelfArticleListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _MpSelfArticleListExcelExporter = MpSelfArticleListExcelExporter;
            _userMpAppService = userMpAppService;
        }

        protected override IQueryable<MpSelfArticle> CreateFilteredQuery(GetMpSelfArticlesInput input)
        {
            
            return Repository.GetAll()
                .Where( c => c.MpID == input.MpID)
                  .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Title))
                   .WhereIf(!input.Description.IsNullOrWhiteSpace(), c => c.Description.Contains(input.Description));
        }
        public async Task<FileDto> GetListToExcel(GetMpSelfArticlesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpSelfArticleListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpSelfArticleDto> GetModelByReplyTypeAsync(int id, int mpId)
        {
            CheckGetPermission();

            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll().Where(m => m.IsDeleted == false).WhereIf(id!=0, c => c.Id == id).WhereIf(mpId!=0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

        public async Task<List<MpSelfArticleDto>> GetListByIds(List<int> ids)
        {
            return (await Repository.GetAllListAsync(m => ids.Contains(m.Id))).Select(MapToEntityDto).ToList();
        }
    }
}
