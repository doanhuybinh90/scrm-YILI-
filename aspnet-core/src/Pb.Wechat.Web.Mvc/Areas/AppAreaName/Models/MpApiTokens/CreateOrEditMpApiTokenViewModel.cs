using Abp.AutoMapper;
using Pb.Wechat.MpApiTokens.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpApiTokens
{
    [AutoMapFrom(typeof(MpApiTokenDto))]
    public class CreateOrEditMpApiTokenViewModel: MpApiTokenDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditMpApiTokenViewModel(MpApiTokenDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpApiTokenViewModel()
        {
        }
    }
}
