using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pb.Wechat.Dto;
using Pb.Wechat.MpCourseSignups.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pb.Wechat.MpCourseSignups
{
    public interface IMpCourseSignupAppService : IAsyncCrudAppService<MpCourseSignupDto, int, GetMpCourseSignupsInput, MpCourseSignupDto, MpCourseSignupDto>
    {
        Task<FileDto> GetListToExcel(GetMpCourseSignupsInput input);

        Task<MpCourseSignupDto> GetFirstOrDefaultByInput(GetMpCourseSignupsInput input);
        Task<List<MpCourseSignupDto>> GetList();
    }
}
