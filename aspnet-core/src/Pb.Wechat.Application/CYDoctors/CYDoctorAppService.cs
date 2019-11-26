using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.CYDoctors.Dto;
using Pb.Wechat.CYDoctors.Exporting;
using System.Linq;
using System.Threading.Tasks;
using Pb.WeChat.CYDoctors.Dto;

namespace Pb.Wechat.CYDoctors
{
    public class CYDoctorAppService : AsyncCrudAppService<CYDoctor, CYDoctorDto, int, GetCYDoctorsInput, CYDoctorDto, CYDoctorDto>, ICYDoctorAppService
    {
        private readonly ICYDoctorListExcelExporter _cyDoctorListExcelExporter;
        public CYDoctorAppService(IRepository<CYDoctor, int> repository, ICYDoctorListExcelExporter cyDoctorListExcelExporter) : base(repository)
        {
            _cyDoctorListExcelExporter = cyDoctorListExcelExporter;
        }

        protected override IQueryable<CYDoctor> CreateFilteredQuery(GetCYDoctorsInput input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Keyword));
        }
        public async Task<FileDto> GetListToExcel(GetCYDoctorsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _cyDoctorListExcelExporter.ExportToFile(dtos);
        }
        public async Task<CYDoctorDto> GetByCyId(string id)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(c => c.CYId == id)));
        }
    }
}
