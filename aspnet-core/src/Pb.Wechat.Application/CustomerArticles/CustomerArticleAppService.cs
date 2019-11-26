using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerArticles.Exporting;
using Pb.Wechat.UserMps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Pb.Wechat.CustomerServiceResponseTexts;
using System;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Abp.UI;
using Pb.Wechat.CustomerArticleGroupItems;

namespace Pb.Wechat.CustomerArticles.Dto.CustomerArticles
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class CustomerArticleAppService : AsyncCrudAppService<CustomerArticle, CustomerArticleDto, int, GetCustomerArticlesInput, CustomerArticleDto, CustomerArticleDto>, ICustomerArticleAppService
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly ICustomerArticleListExcelExporter _CustomerArticleListExcelExporter;
        private readonly IRepository<CustomerServiceResponseText, int> _cusRepository;
        private readonly IRepository<CustomerArticleGroupItem, int> _itemRepository;
        public CustomerArticleAppService(IRepository<CustomerArticle, int> repository, ICustomerArticleListExcelExporter CustomerArticleListExcelExporter, IUserMpAppService userMpAppService, IRepository<CustomerServiceResponseText, int> cusRepository, IRepository<CustomerArticleGroupItem, int> itemRepository) : base(repository)
        {
            _CustomerArticleListExcelExporter = CustomerArticleListExcelExporter;
            _userMpAppService = userMpAppService;
            _cusRepository = cusRepository;
            _itemRepository = itemRepository;
        }

        protected override IQueryable<CustomerArticle> CreateFilteredQuery(GetCustomerArticlesInput input)
        {
            
            return Repository.GetAll()
                .Where( c => c.MpID == input.MpID)
                  .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Title))
                   .WhereIf(!input.Description.IsNullOrWhiteSpace(), c => c.Description.Contains(input.Description));
        }
        public async Task<FileDto> GetListToExcel(GetCustomerArticlesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _CustomerArticleListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CustomerArticleDto> GetModelByReplyTypeAsync(int id, int mpId)
        {
            CheckGetPermission();

            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll().Where(m => m.IsDeleted == false).WhereIf(id!=0, c => c.Id == id).WhereIf(mpId!=0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

        public async Task<List<CustomerArticleDto>> GetListByIds(List<int> ids)
        {
            return (await Repository.GetAllListAsync(m => ids.Contains(m.Id))).Select(MapToEntityDto).ToList();
        }

        public override async Task<CustomerArticleDto> Create(CustomerArticleDto input)
        {
            var result = await base.Create(input);
            await _cusRepository.InsertAsync(new CustomerServiceResponseText
            {
          
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LastModificationTime = DateTime.Now,
                MediaId = input.MediaID,
                MpID = input.MpID,
                PreviewImgUrl = input.FilePathOrUrl,
                MartialId = result.Id,
                ResponseType = ResponseType.common.ToString(),
                ReponseContentType = (int)CustomerServiceMsgType.mpnews,
                TypeId = input.TypeId,
                TypeName = input.TypeName,
                ResponseText = input.Title
            });
            return result;
        }
        public override async Task<CustomerArticleDto> Update(CustomerArticleDto input)
        {
            var result = await base.Update(input);
            var updateModel = await _cusRepository.FirstOrDefaultAsync(m => m.MartialId == result.Id);
            updateModel.Title = input.Title;
            updateModel.Description = input.Description;
            updateModel.LastModificationTime = DateTime.Now;
            updateModel.MediaId = input.MediaID;
            updateModel.PreviewImgUrl = input.FilePathOrUrl;
            updateModel.TypeId = input.TypeId;
            updateModel.TypeName = input.TypeName;
            updateModel.ResponseText = input.Title;
            await _cusRepository.UpdateAsync(updateModel);
           
            return result;
        }
        public override Task Delete(EntityDto<int> input)
        {
            var model = Repository.Get(input.Id);
            if (model != null)
            {
                _itemRepository.Delete(m => m.ArticleID == model.Id);
                _cusRepository.Delete(m => m.MartialId == model.Id);
                return base.Delete(input);
            }

            throw new UserFriendlyException("对不起，删除素材失败");
        }
    }
}
