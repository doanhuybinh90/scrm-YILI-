using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CYProblemContents.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.CYProblemContents
{
    public interface ICYProblemContentAppService : IAsyncCrudAppService<CYProblemContentDto, int, GetCYProblemContentsInput, CYProblemContentDto, CYProblemContentDto>
    {
        Task<FileDto> GetListToExcel(GetCYProblemContentsInput input);

        Task<bool> HasDoctorReply(int cyproblemid);
    }
}
