using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Pb.Wechat.MpMediaArticleGroupItems.Dto;
using Pb.Wechat.MpMediaArticleGroupItems.Exporting;
using Pb.Wechat.MpMediaArticleGroups.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pb.Wechat.Dto;
using Abp.Collections.Extensions;

namespace Pb.Wechat.MpMediaArticleGroupItems
{
    public class MpMediaArticleGroupItemAppService: AsyncCrudAppService<MpMediaArticleGroupItem, MpMediaArticleGroupItemDto,int,GetMpMediaArticleGrouItemInput>, IMpMediaArticleGroupItemAppService
    {
        private readonly IMpMediaArticleGroupItemListExcelExporter _IMpMediaArticleGroupItemListExcelExporter;
        public MpMediaArticleGroupItemAppService(IRepository<MpMediaArticleGroupItem, int> repository, IMpMediaArticleGroupItemListExcelExporter IMpMediaArticleGroupItemListExcelExporter) : base(repository)
        {
            _IMpMediaArticleGroupItemListExcelExporter = IMpMediaArticleGroupItemListExcelExporter;
        }

        //public override Task<PagedResultDto<MpMediaArticleGroupItemDto>> GetAll(GetMpMediaArticleGrouItemInput input)
        //{
        //    return base.GetAll(input);
        //}
        public List<MpMediaArticleGroupItem> GetList()
        {
            return Repository.GetAllList();
        }

        //protected override IQueryable<MpMediaArticleGroupItem> CreateFilteredQuery(GetMpMediaArticleGrouItemInput input)
        //{
        //    return  Repository.GetAll()
        //           .WhereIf(input.GroupID!=0,c=>c.GroupID==input.GroupID);

        //}


        public async Task<FileDto> GetListToExcel(GetMpMediaArticleGrouItemInput input)
        {
            CheckGetAllPermission();
            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _IMpMediaArticleGroupItemListExcelExporter.ExportToFile(dtos);
        }

        //保存
        public void Save(MpMediaArticleGroupItem input)
        {
            Repository.InsertOrUpdate(input);
        }
    }
}
