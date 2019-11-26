using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpApiTokens;
using Pb.Wechat.MpShoppingMallPics.Dto;
using Pb.Wechat.UserMps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpShoppingMallPics
{
    //[AbpAuthorize(AppPermissions.Pages_MpShoppingMallPic)]
    public class MpShoppingMallPicAppService : AsyncCrudAppService<MpShoppingMallPic, MpShoppingMallPicDto, int, GetMpShoppingMallPicsInput, MpShoppingMallPicDto, MpShoppingMallPicDto>, IMpShoppingMallPicAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpShoppingMallPicListExcelExporter _MpShoppingMallPicListExcelExporter;
        private readonly IRepository<MpApiToken, int> _apiTokenRepository;
        public MpShoppingMallPicAppService(IRepository<MpShoppingMallPic, int> repository, IMpShoppingMallPicListExcelExporter MpShoppingMallPicListExcelExporter, IUserMpAppService userMpAppService,  IRepository<MpApiToken, int> apiTokenRepository) : base(repository)
        {
            _MpShoppingMallPicListExcelExporter = MpShoppingMallPicListExcelExporter;
            _userMpAppService = userMpAppService;
            _apiTokenRepository = apiTokenRepository;
        }

        protected override IQueryable<MpShoppingMallPic> CreateFilteredQuery(GetMpShoppingMallPicsInput input)
        {
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Title), c => c.Name.Contains(input.Title));
        }
        public override Task<MpShoppingMallPicDto> Create(MpShoppingMallPicDto input)
        {
            input.LastModificationTime = DateTime.Now;
            return base.Create(input);
        }

        public async Task<FileDto> GetListToExcel(GetMpShoppingMallPicsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpShoppingMallPicListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpShoppingMallPicDto> GetFirstOrDefaultByInput(GetMpShoppingMallPicsInput input)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input)));
        }

        public async Task<List<MpShoppingMallPicDto>> GetListByNames(string token,params string[] name)
        {
            var tokenModel = await _apiTokenRepository.FirstOrDefaultAsync(m => m.Token == token && m.IsDeleted == false);
            List<MpShoppingMallPicDto> datas = null;
            if (name.Count()>0)
            {
                datas = (await Repository.GetAllListAsync(m => m.IsDeleted == false && m.MpID == tokenModel.ParentId && name.Contains(m.Name))).Select(MapToEntityDto).ToList();
            }
            else
                datas=(await Repository.GetAllListAsync(m => m.IsDeleted == false && m.MpID == tokenModel.ParentId)).Select(MapToEntityDto).ToList();
            return datas;
        }
    }
}
