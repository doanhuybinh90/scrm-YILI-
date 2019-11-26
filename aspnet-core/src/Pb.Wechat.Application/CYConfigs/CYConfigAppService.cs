using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.CYConfigs.Dto;
using Pb.Wechat.CYConfigs.Exporting;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.CYConfigs
{
    public class CYConfigAppService : AsyncCrudAppService<CYConfig, CYConfigDto, int, GetCYConfigsInput, CYConfigDto, CYConfigDto>, ICYConfigAppService
    {
        private readonly ICYConfigListExcelExporter _cYConfigListExcelExporter;
        public CYConfigAppService(IRepository<CYConfig, int> repository, ICYConfigListExcelExporter cYConfigListExcelExporter) : base(repository)
        {
            _cYConfigListExcelExporter = cYConfigListExcelExporter;
        }

        protected override IQueryable<CYConfig> CreateFilteredQuery(GetCYConfigsInput input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.NoOperationText.Contains(input.Keyword));
        }
        public async Task<FileDto> GetListToExcel(GetCYConfigsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _cYConfigListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CYConfigDto> GetConfig() {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()));
        }
    }
}
