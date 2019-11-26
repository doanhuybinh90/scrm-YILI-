using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.CYProblemContents.Dto;
using Pb.Wechat.CYProblemContents.Exporting;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.CYProblems.Dto;

namespace Pb.Wechat.CYProblemContents
{
    public class CYProblemContentAppService : AsyncCrudAppService<CYProblemContent, CYProblemContentDto, int, GetCYProblemContentsInput, CYProblemContentDto, CYProblemContentDto>, ICYProblemContentAppService
    {
        private readonly ICYProblemContentListExcelExporter _cyProblemContentListExcelExporter;
        public CYProblemContentAppService(IRepository<CYProblemContent, int> repository, ICYProblemContentListExcelExporter cyProblemContentListExcelExporter) : base(repository)
        {
            _cyProblemContentListExcelExporter = cyProblemContentListExcelExporter;
        }

        protected override IQueryable<CYProblemContent> CreateFilteredQuery(GetCYProblemContentsInput input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Text.Contains(input.Keyword));
        }
        public async Task<FileDto> GetListToExcel(GetCYProblemContentsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _cyProblemContentListExcelExporter.ExportToFile(dtos);
        }

        public async Task<bool> HasDoctorReply(int problemid)
        {
            var sender = (int)CYProblemContentSender.doctor;
            return await Repository.CountAsync(c => c.ProblemId == problemid && c.SendUser == sender) > 0;
        }
    }
}
