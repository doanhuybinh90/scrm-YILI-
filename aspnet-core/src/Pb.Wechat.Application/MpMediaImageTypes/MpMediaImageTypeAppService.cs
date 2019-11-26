using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaImageTypes.Dto;
using Pb.Wechat.MpMediaImageTypes.Exporting;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaImageTypes
{
    public class MpMediaImageTypeAppService : AsyncCrudAppService<MpMediaImageType, MpMediaImageTypeDto, int, GetMpMediaImageTypesInput, MpMediaImageTypeDto, MpMediaImageTypeDto>, IMpMediaImageTypeAppService
    {
        private readonly IMpMediaImageTypeListExcelExporter _MpMediaImageTypeListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaAppService _wxMediaAppService;
        public MpMediaImageTypeAppService(IRepository<MpMediaImageType, int> repository, IMpMediaImageTypeListExcelExporter MpMediaImageTypeListExcelExporter, IUserMpAppService userMpAppService, IWxMediaAppService wxMediaAppService) : base(repository)
        {
            _MpMediaImageTypeListExcelExporter = MpMediaImageTypeListExcelExporter;
            _userMpAppService = userMpAppService;
            _wxMediaAppService = wxMediaAppService;
        }

        protected override IQueryable<MpMediaImageType> CreateFilteredQuery(GetMpMediaImageTypesInput input)
        {

            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                 .WhereIf(!input.MediaTypeName.IsNullOrWhiteSpace(), c => c.MediaTypeName.Contains(input.MediaTypeName))
                 ;
        }

       
        public async Task<FileDto> GetListToExcel(GetMpMediaImageTypesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpMediaImageTypeListExcelExporter.ExportToFile(dtos);
        }

       public async Task<List<NameValue<string>>> GetAllList(GetMpMediaImageTypesInput input)
        {
            var result = new List<NameValue<string>>();
            var datas = (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.MpID == input.MpID && m.IsDeleted == false )))
                .Select(m => new { m.Id, m.MediaTypeName }).Distinct().ToList();
            foreach (var data in datas)
            {
                var value = new NameValue() { Value = data.Id.ToString(), Name = data.MediaTypeName };
                result.Add(value);
            }
            return result;
        }

        
    }
}
