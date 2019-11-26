using Abp.AutoMapper;
using Pb.Wechat.MpShoppingMallPics.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpShoppingMallPics
{
    [AutoMapFrom(typeof(MpShoppingMallPicDto))]
    public class CreateOrEditMpShoppingMallPicViewModel : MpShoppingMallPicDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditMpShoppingMallPicViewModel(MpShoppingMallPicDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpShoppingMallPicViewModel() {
        }
    }
}
