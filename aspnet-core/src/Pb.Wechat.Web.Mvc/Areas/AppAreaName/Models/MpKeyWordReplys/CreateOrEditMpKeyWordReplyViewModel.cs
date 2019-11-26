using Abp.AutoMapper;
using Pb.Wechat.MpKeyWordReplys.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpKeyWordReplys
{
    [AutoMapFrom(typeof(MpKeyWordReplyDto))]
    public class CreateOrEditMpKeyWordReplyViewModel : MpKeyWordReplyDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpKeyWordReplyViewModel(MpKeyWordReplyDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpKeyWordReplyViewModel() {
        }
    }
}
