using Abp.AutoMapper;
using Pb.Wechat.MpAccounts.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpAccounts
{
    [AutoMapFrom(typeof(MpAccountDto))]
    public class CreateOrEditMpAccountViewModel : MpAccountDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditMpAccountViewModel(MpAccountDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpAccountViewModel() {
        }
    }
}
