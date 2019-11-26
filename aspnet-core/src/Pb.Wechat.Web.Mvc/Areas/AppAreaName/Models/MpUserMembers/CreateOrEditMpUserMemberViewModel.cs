using Abp.AutoMapper;
using Pb.Wechat.MpUserMembers.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpUserMembers
{
    [AutoMapFrom(typeof(MpUserMemberDto))]
    public class CreateOrEditMpUserMemberViewModel : MpUserMemberDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditMpUserMemberViewModel(MpUserMemberDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpUserMemberViewModel() {
        }
    }
}
