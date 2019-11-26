using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.MpFansTags.Dto;
using Pb.Wechat.MpFansTags.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpFansTags
{
    public class MpFansTagAppService : AsyncCrudAppService<MpFansTag, MpFansTagDto, int, GetMpFansTagsInput, MpFansTagDto, MpFansTagDto>, IMpFansTagAppService
    {
        private readonly IMpFansTagListExcelExporter _mpFansTagListExcelExporter;
        public MpFansTagAppService(IRepository<MpFansTag, int> repository, IMpFansTagListExcelExporter mpFansTagListExcelExporter) : base(repository)
        {
            _mpFansTagListExcelExporter = mpFansTagListExcelExporter;
        }

        protected override IQueryable<MpFansTag> CreateFilteredQuery(GetMpFansTagsInput input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Keyword));
        }
        public async Task<FileDto> GetListToExcel(GetMpFansTagsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpFansTagListExcelExporter.ExportToFile(dtos);
        }

        public async Task<IList<MpFansTagDto>> GetAllTags(GetMpFansTagsInput input)
        {
            return (await AsyncQueryableExecuter.ToListAsync(CreateFilteredQuery(input))).Select(MapToEntityDto).ToList(); ;
        }
    }
}
