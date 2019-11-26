using Abp.AutoMapper;
using Pb.Wechat.CYDoctors.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CYDoctors
{
    [AutoMapFrom(typeof(CYDoctorDto))]
    public class CreateOrEditCYDoctorViewModel : CYDoctorDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCYDoctorViewModel(CYDoctorDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCYDoctorViewModel()
        {
        }
    }
}
