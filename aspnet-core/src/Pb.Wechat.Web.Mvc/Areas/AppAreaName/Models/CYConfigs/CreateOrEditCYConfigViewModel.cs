using Abp.AutoMapper;
using Pb.Wechat.CYConfigs.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CYConfigs
{
    [AutoMapFrom(typeof(CYConfigDto))]
    public class CreateOrEditCYConfigViewModel : CYConfigDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCYConfigViewModel(CYConfigDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCYConfigViewModel()
        {
        }
    }
}
