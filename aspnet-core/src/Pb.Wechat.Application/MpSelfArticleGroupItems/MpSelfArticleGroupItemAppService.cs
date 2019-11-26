using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSelfArticleGroupItems.Dto;
using Pb.Wechat.MpSelfArticleGroupItems.Exporting;
using Pb.Wechat.UserMps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpSelfArticleGroupItems
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class MpSelfArticleGroupItemAppService : AsyncCrudAppService<MpSelfArticleGroupItem, MpSelfArticleGroupItemDto, int, GetMpSelfArticleGroupItemsInput, MpSelfArticleGroupItemDto, MpSelfArticleGroupItemDto>, IMpSelfArticleGroupItemAppService
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpSelfArticleGroupItemListExcelExporter _MpSelfArticleGroupItemListExcelExporter;
        public MpSelfArticleGroupItemAppService(IRepository<MpSelfArticleGroupItem, int> repository, IMpSelfArticleGroupItemListExcelExporter MpSelfArticleGroupItemListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _MpSelfArticleGroupItemListExcelExporter = MpSelfArticleGroupItemListExcelExporter;
            _userMpAppService = userMpAppService;
        }

        protected override IQueryable<MpSelfArticleGroupItem> CreateFilteredQuery(GetMpSelfArticleGroupItemsInput input)
        {
           
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(input.ArticleID != 0, c => c.ArticleID == input.ArticleID)
                .WhereIf(input.GroupID != 0, c => c.GroupID == input.GroupID);
        }
        public async Task<FileDto> GetListToExcel(GetMpSelfArticleGroupItemsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpSelfArticleGroupItemListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpSelfArticleGroupItemDto> GetModelByReplyTypeAsync(int id, int mpId)
        {
            CheckGetPermission();
            
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(id != 0, c => c.Id == id)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

        public async Task<bool> SaveItems(List<MpSelfArticleGroupItemDto> inputs)
        {
            if (inputs != null && inputs.Count > 0)
            {
                var groupId = inputs.First().GroupID;
                await base.Repository.DeleteAsync(m => m.GroupID == groupId);
                foreach (var input in inputs)
                {
                    await base.Create(input);
                }
                return true;
            }
            return false;

        }

        public async Task<PagedResultDto<MpSelfArticleGroupItemDto>> GetGroupItemList(EntityDto<int> GroupID)
        {

            var datas = (await Repository.GetAllListAsync(m => m.IsDeleted == false && m.GroupID == GroupID.Id)).OrderBy(m => m.SortIndex).ToList();
            var List = ObjectMapper.Map<List<MpSelfArticleGroupItemDto>>(datas);
            return new PagedResultDto<MpSelfArticleGroupItemDto>(List.Count, List);

        }

        public async Task<List<MpSelfArticleGroupItemDto>> GetGroupItems(int groupId)
        {
            return (await Repository.GetAllListAsync(m => m.IsDeleted == false && m.GroupID == groupId)).OrderBy(m => m.SortIndex).Select(MapToEntityDto).ToList();
        }
       

        public async Task DeleteByGroupID(int groupId)
        {
            await Repository.DeleteAsync(m => m.GroupID == groupId);
        }
    }
}
