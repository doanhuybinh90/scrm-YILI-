using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.MpFansTagItems.Dto;
using Pb.Wechat.MpFansTagItems.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpFansTagItems
{
    public class MpFansTagItemAppService : AsyncCrudAppService<MpFansTagItem, MpFansTagItemDto, long, GetMpFansTagItemsInput, MpFansTagItemDto, MpFansTagItemDto>, IMpFansTagItemAppService
    {
        private readonly IMpFansTagItemListExcelExporter _mpFansTagItemListExcelExporter;
        public MpFansTagItemAppService(IRepository<MpFansTagItem, long> repository, IMpFansTagItemListExcelExporter mpFansTagItemListExcelExporter) : base(repository)
        {
            _mpFansTagItemListExcelExporter = mpFansTagItemListExcelExporter;
        }

        protected override IQueryable<MpFansTagItem> CreateFilteredQuery(GetMpFansTagItemsInput input)
        {
            return Repository.GetAll()
                .WhereIf(input.MpId.HasValue, c => c.MpID == input.MpId.Value)
                .WhereIf(input.FansId.HasValue, c => c.FansId == input.FansId.Value)
                .WhereIf(input.TagIds != null && input.TagIds.Length > 0, c => input.TagIds.Contains(c.TagId));
        }
        public async Task<FileDto> GetListToExcel(GetMpFansTagItemsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpFansTagItemListExcelExporter.ExportToFile(dtos);
        }

        public async Task SaveFansTags(int mpId, int fansId, string tagIds) {
            if (!string.IsNullOrEmpty(tagIds))
            {
                long tid = -1;
                var tagids = new List<long>();
                foreach (var item in tagIds.Split(',')) {
                    if (long.TryParse(item, out tid))
                        tagids.Add(tid);
                }
                var existsTags = await AsyncQueryableExecuter.ToListAsync(CreateFilteredQuery(new GetMpFansTagItemsInput() {
                    MpId=mpId,
                    FansId=fansId,
                    TagIds=tagids.ToArray()
                }));

                foreach (var item in tagids.Except(existsTags.Select(c => (long)c.TagId))) {
                    await Repository.InsertAsync(new MpFansTagItem()
                    {
                        MpID = mpId,
                        FansId = fansId,
                        TagId = (int)item,
                        CreationTime = DateTime.Now,
                        IsDeleted = false
                    });
                }
            }
            
        }
    }
}
