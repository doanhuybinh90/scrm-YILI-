using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CYProblems.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.CYProblems
{
    public interface ICYProblemAppService : IAsyncCrudAppService<CYProblemDto, int, GetCYProblemsInput, CYProblemDto, CYProblemDto>
    {
        Task<FileDto> GetListToExcel(GetCYProblemsInput input);

        Task<CYProblemDto> GetUserLastProblem(string openid);

        Task<CYProblemDto> GetByCyId(int id);
    }
}
