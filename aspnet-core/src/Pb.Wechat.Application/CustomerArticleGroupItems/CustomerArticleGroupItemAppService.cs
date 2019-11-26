using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerArticleGroupItems.Dto;
using Pb.Wechat.CustomerArticleGroupItems.Exporting;
using Pb.Wechat.UserMps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.CustomerArticleGroupItems;

namespace Pb.Wechat.CustomerArticleGroupItems
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class CustomerArticleGroupItemAppService : AsyncCrudAppService<CustomerArticleGroupItem, CustomerArticleGroupItemDto, int, GetCustomerArticleGroupItemsInput, CustomerArticleGroupItemDto, CustomerArticleGroupItemDto>, ICustomerArticleGroupItemAppService
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly ICustomerArticleGroupItemListExcelExporter _CustomerArticleGroupItemListExcelExporter;
        public CustomerArticleGroupItemAppService(IRepository<CustomerArticleGroupItem, int> repository, ICustomerArticleGroupItemListExcelExporter CustomerArticleGroupItemListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _CustomerArticleGroupItemListExcelExporter = CustomerArticleGroupItemListExcelExporter;
            _userMpAppService = userMpAppService;
        }

        protected override IQueryable<CustomerArticleGroupItem> CreateFilteredQuery(GetCustomerArticleGroupItemsInput input)
        {
           
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(input.ArticleID != 0, c => c.ArticleID == input.ArticleID)
                .WhereIf(input.GroupID != 0, c => c.GroupID == input.GroupID);
        }
        public async Task<FileDto> GetListToExcel(GetCustomerArticleGroupItemsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _CustomerArticleGroupItemListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CustomerArticleGroupItemDto> GetModelByReplyTypeAsync(int id, int mpId)
        {
            CheckGetPermission();
            
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(id != 0, c => c.Id == id)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

        public async Task<bool> SaveItems(List<CustomerArticleGroupItemDto> inputs)
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

        public async Task<PagedResultDto<CustomerArticleGroupItemDto>> GetGroupItemList(EntityDto<int> GroupID)
        {

            var datas = (await Repository.GetAllListAsync(m => m.IsDeleted == false && m.GroupID == GroupID.Id)).OrderBy(m => m.SortIndex).ToList();
            var List = ObjectMapper.Map<List<CustomerArticleGroupItemDto>>(datas);
            return new PagedResultDto<CustomerArticleGroupItemDto>(List.Count, List);

        }

        public async Task<List<CustomerArticleGroupItemDto>> GetGroupItems(int groupId)
        {
            return (await Repository.GetAllListAsync(m => m.IsDeleted == false && m.GroupID == groupId)).OrderBy(m => m.SortIndex).Select(MapToEntityDto).ToList();
        }
       

        public async Task DeleteByGroupID(int groupId)
        {
            await Repository.DeleteAsync(m => m.GroupID == groupId);
        }
    }
}
