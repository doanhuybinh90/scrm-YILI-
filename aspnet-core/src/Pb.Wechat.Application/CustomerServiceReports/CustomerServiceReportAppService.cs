using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceReports.Dto;
using Pb.Wechat.CustomerServiceReports.Exporting;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;

namespace Pb.Wechat.CustomerServiceReports
{
    public class CustomerServiceReportAppService : AsyncCrudAppService<CustomerServiceReport, CustomerServiceReportDto, long, GetCustomerServiceReportsInput, CustomerServiceReportDto, CustomerServiceReportDto>, ICustomerServiceReportAppService
    {
        private readonly ICustomerServiceReportListExcelExporter _CustomerServiceReportListExcelExporter;
        public CustomerServiceReportAppService(IRepository<CustomerServiceReport, long> repository, ICustomerServiceReportListExcelExporter CustomerServiceReportListExcelExporter) : base(repository)
        {
            _CustomerServiceReportListExcelExporter = CustomerServiceReportListExcelExporter;
        }

        protected override IQueryable<CustomerServiceReport> CreateFilteredQuery(GetCustomerServiceReportsInput input)
        {
            return Repository.GetAll()
                .WhereIf(input.MpID != 0, c => c.MpID == input.MpID)
                .WhereIf(!input.NickName.IsNullOrWhiteSpace(), c => c.NickName.Contains(input.NickName))
                .WhereIf(input.StatistStartDate.HasValue, c => c.ReportDate >= input.StatistStartDate.Value)
                .WhereIf(input.StatistEndDate.HasValue, c => c.ReportDate <= input.StatistEndDate.Value);
            //.WhereIf(input.CustomerIds!=null && input.CustomerIds.Count>0,c=>input.CustomerIds.Contains(c.CustomerId));

        }
      
        public override async Task<PagedResultDto<CustomerServiceReportDto>> GetAll(GetCustomerServiceReportsInput input)
        {
            if (input.NeedSum == "0")
            {
                CheckGetAllPermission();
                var query = CreateFilteredQuery(input);
                query = ApplySorting(query, input);
                try
                {
                    var totalCount = query.Count();
                    var pagedInput = input as IPagedResultRequest;
                    if (pagedInput != null)
                    {
                        query = query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
                    }
                    var resultList = query.Select(MapToEntityDto).ToList();
                    return
                        new PagedResultDto<CustomerServiceReportDto>
                        {
                            TotalCount = totalCount,
                            Items = resultList
                        };
                }
                catch (System.Exception ex)
                {

                    throw ex;
                }
                

               

            }
            else
            {
                CheckGetAllPermission();
                var query = CreateFilteredQuery(input).GroupBy(m => m.CustomerId).Select(m => new CustomerServiceReportDto
                {
                    MpID = m.Max(t => t.MpID),
                    AvgScore = m.Average(t => t.AvgScore),
                    CustomerId = m.Key,
                    NickName = m.Max(t => t.NickName),
                    OnlineTime = m.Sum(t => t.OnlineTime),
                    ReceiveCount = m.Sum(t => t.ReceiveCount),
                    ReportDate = m.Max(t => t.ReportDate),
                    ScoreCount = m.Sum(t => t.ScoreCount),
                    ServiceCount = m.Sum(t => t.ServiceCount),
                    ServiceMsgCount = m.Sum(t => t.ServiceMsgCount),
                    TotalScore = m.Sum(t => t.TotalScore),
                    CreationTime = m.Max(t => t.CreationTime),
                    Id = m.Max(t => t.Id)
                });
                var totalCount = query.Count();

                var pagedInput = input as IPagedResultRequest;
                if (pagedInput != null)
                {
                    query = query.Skip(pagedInput.SkipCount).Take(pagedInput.MaxResultCount);
                }
                var resultList = query.ToList();
                return
                    new PagedResultDto<CustomerServiceReportDto>
                    {
                        TotalCount = totalCount,
                        Items = resultList
                    };

            }
        }

        public async Task<FileDto> GetListToExcel(GetCustomerServiceReportsInput input)
        {
            CheckGetAllPermission();

            if (input.NeedSum == "0")
            {
                var query = CreateFilteredQuery(input);
                query = ApplySorting(query, input);
                var entities = await AsyncQueryableExecuter.ToListAsync(query);
                var dtos = entities.Select(MapToEntityDto).ToList();
                return _CustomerServiceReportListExcelExporter.ExportToFile(dtos);
            }
            else
            {
                var query = CreateFilteredQuery(input).GroupBy(m => m.CustomerId).Select(m => new CustomerServiceReportDto
                {
                    MpID = m.Max(t => t.MpID),
                    AvgScore = m.Average(t => t.AvgScore),
                    CustomerId = m.Key,
                    NickName = m.Max(t => t.NickName),
                    OnlineTime = m.Sum(t => t.OnlineTime),
                    ReceiveCount = m.Sum(t => t.ReceiveCount),
                    ReportDate = m.Max(t => t.ReportDate),
                    ScoreCount = m.Sum(t => t.ScoreCount),
                    ServiceCount = m.Sum(t => t.ServiceCount),
                    ServiceMsgCount = m.Sum(t => t.ServiceMsgCount),
                    TotalScore = m.Sum(t => t.TotalScore),
                    CreationTime = m.Max(t => t.CreationTime),
                    Id = m.Max(t => t.Id)
                });
                var dtos = query.ToList();
                return _CustomerServiceReportListExcelExporter.ExportToFile(dtos);
            }
        
        }


    }
}
