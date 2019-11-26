using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceResponseTypes.Dto;
using Pb.Wechat.UserMps;
using System;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.CustomerServiceWorkTimes;
using Abp;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerServiceResponseTypes
{
    //[AbpAuthorize(AppPermissions.Pages_CustomerServiceResponseTypes)]
    public class CustomerServiceResponseTypeAppService : AsyncCrudAppService<CustomerServiceResponseType, CustomerServiceResponseTypeDto, int, GetCustomerServiceResponseTypesInput, CustomerServiceResponseTypeDto, CustomerServiceResponseTypeDto>, ICustomerServiceResponseTypeAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly ICustomerServiceResponseTypeListExcelExporter _CustomerServiceResponseTypeListExcelExporter;
        private readonly ICustomerServiceWorkTimeAppService _customerServiceWorkTimeAppService;
        public CustomerServiceResponseTypeAppService(IRepository<CustomerServiceResponseType, int> repository, ICustomerServiceResponseTypeListExcelExporter CustomerServiceResponseTypeListExcelExporter, IUserMpAppService userMpAppService,  ICustomerServiceWorkTimeAppService customerServiceWorkTimeAppService) : base(repository)
        {
            _CustomerServiceResponseTypeListExcelExporter = CustomerServiceResponseTypeListExcelExporter;
            _userMpAppService = userMpAppService;
            _customerServiceWorkTimeAppService = customerServiceWorkTimeAppService;

        }

        protected override IQueryable<CustomerServiceResponseType> CreateFilteredQuery(GetCustomerServiceResponseTypesInput input)
        {
            return Repository.GetAll()
                .WhereIf(input.Id != null, c => c.Id==input.Id)
                .WhereIf(input.TypeDescription != null, c => c.TypeDescription.Contains(input.TypeDescription))
               ;
        }
        public override async Task<CustomerServiceResponseTypeDto> Update(CustomerServiceResponseTypeDto input)
        {
            input.LastModificationTime = DateTime.Now;
            return await base.Update(input);
        }
        public override async Task<CustomerServiceResponseTypeDto> Create(CustomerServiceResponseTypeDto input)
        {
            input.LastModificationTime = DateTime.Now;
            return await base.Create(input);
        }
        public async Task<FileDto> GetListToExcel(GetCustomerServiceResponseTypesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _CustomerServiceResponseTypeListExcelExporter.ExportToFile(dtos);
        }

        public async Task<List<NameValue<string>>> GetTypeSelected()
        {
            var result = new List<NameValue<string>>();
            var datas = Repository.GetAllList().Select(m => new { Id = m.Id, Name = m.TypeDescription }).ToList();
            foreach (var data in datas)
            {
                var value = new NameValue() { Value = data.Id.ToString(), Name = data.Name };
                result.Add(value);
            }
            return result;
        }
    }
}
