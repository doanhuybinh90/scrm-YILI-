using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.CYProblems.Dto;
using Pb.Wechat.CYProblems.Exporting;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.CYProblems
{
    public class CYProblemAppService : AsyncCrudAppService<CYProblem, CYProblemDto, int, GetCYProblemsInput, CYProblemDto, CYProblemDto>, ICYProblemAppService
    {
        private readonly ICYProblemListExcelExporter _cyProblemListExcelExporter;
        public CYProblemAppService(IRepository<CYProblem, int> repository, ICYProblemListExcelExporter cyProblemListExcelExporter) : base(repository)
        {
            _cyProblemListExcelExporter = cyProblemListExcelExporter;
        }

        protected override IQueryable<CYProblem> CreateFilteredQuery(GetCYProblemsInput input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.OpenId.Contains(input.Keyword));
        }
        public async Task<FileDto> GetListToExcel(GetCYProblemsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _cyProblemListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CYProblemDto> GetUserLastProblem(string openid) {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(c => c.OpenId == openid).OrderByDescending(c => c.CreationTime)));
        }
        public async Task<CYProblemDto> GetByCyId(int id) {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(c => c.CYProblemId == id).OrderByDescending(c => c.CreationTime)));
        }
    }
}
