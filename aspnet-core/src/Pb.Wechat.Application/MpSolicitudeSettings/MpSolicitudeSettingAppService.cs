using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSolicitudeSettings.Dto;
using Pb.Wechat.MpSolicitudeSettings.Exporting;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSolicitudeSettings
{
    public class MpSolicitudeSettingAppService : AsyncCrudAppService<MpSolicitudeSetting, MpSolicitudeSettingDto, int, GetMpSolicitudeSettingsInput, MpSolicitudeSettingDto, MpSolicitudeSettingDto>, IMpSolicitudeSettingAppService
    {
        private readonly IMpSolicitudeSettingListExcelExporter _mpSolicitudeSettingListExcelExporter;
        public MpSolicitudeSettingAppService(IRepository<MpSolicitudeSetting, int> repository, IMpSolicitudeSettingListExcelExporter mpSolicitudeSettingListExcelExporter) : base(repository)
        {
            _mpSolicitudeSettingListExcelExporter = mpSolicitudeSettingListExcelExporter;
        }

        protected override IQueryable<MpSolicitudeSetting> CreateFilteredQuery(GetMpSolicitudeSettingsInput input)
        {
            return Repository.GetAll();
        }
        public async Task<FileDto> GetListToExcel(GetMpSolicitudeSettingsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpSolicitudeSettingListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpSolicitudeSettingDto> GetDefault()
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()));
        }
    }
}
