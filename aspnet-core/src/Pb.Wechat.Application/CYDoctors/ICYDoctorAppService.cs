using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CYDoctors.Dto;
using System.Threading.Tasks;
using Pb.WeChat.CYDoctors.Dto;

namespace Pb.Wechat.CYDoctors
{
    public interface ICYDoctorAppService : IAsyncCrudAppService<CYDoctorDto, int, GetCYDoctorsInput, CYDoctorDto, CYDoctorDto>
    {
        Task<FileDto> GetListToExcel(GetCYDoctorsInput input);
        Task<CYDoctorDto> GetByCyId(string id);
    }
}
