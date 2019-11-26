using Abp;
using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.MpProductInfos;
using Pb.Wechat.MpProductTypes.Dto;
using Pb.Wechat.MpProductTypes.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpProductTypes
{
    public class MpProductTypeAppService : AsyncCrudAppService<MpProductType, MpProductTypeDto, int, GetMpProductTypesInput, MpProductTypeDto, MpProductTypeDto>, IMpProductTypeAppService
    {
        private readonly IMpProductTypeListExcelExporter _mpProductTypeListExcelExporter;
        private readonly IRepository<MpProductInfo, int> _mpProductInfoRepository;
        public MpProductTypeAppService(IRepository<MpProductType, int> repository, IRepository<MpProductInfo, int> mpProductInfoRepository, IMpProductTypeListExcelExporter mpProductTypeListExcelExporter) : base(repository)
        {
            _mpProductTypeListExcelExporter = mpProductTypeListExcelExporter;
            _mpProductInfoRepository = mpProductInfoRepository;
        }

        protected override IQueryable<MpProductType> CreateFilteredQuery(GetMpProductTypesInput input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Keyword)|| c.SubTitle.Contains(input.Keyword));
        }
        public async Task<FileDto> GetListToExcel(GetMpProductTypesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpProductTypeListExcelExporter.ExportToFile(dtos);
        }

        public override async Task<MpProductTypeDto> Update(MpProductTypeDto input)
        {
            var oldEntity = await Repository.GetAsync(input.Id);
            if (oldEntity.Title != input.Title)
            {
                var subEntities = await AsyncQueryableExecuter.ToListAsync(_mpProductInfoRepository.GetAll().Where(c => c.TypeId == oldEntity.Id));
                foreach (var item in subEntities)
                {
                    item.TypeTitle = input.Title;
                    await _mpProductInfoRepository.UpdateAsync(item);
                }
            }
            return await base.Update(input);
        }

        public async Task<List<NameValue<string>>> GetTypes()
        {
            var result = new List<NameValue<string>>();
            var datas = Repository.GetAllList().OrderBy(c => c.SortIndex).Select(m => new { Id = m.Id, Name = m.Title }).ToList();
            foreach (var data in datas)
            {
                var value = new NameValue() { Value = data.Id.ToString(), Name = data.Name };
                result.Add(value);
            }
            return result;
        }
    }
}
