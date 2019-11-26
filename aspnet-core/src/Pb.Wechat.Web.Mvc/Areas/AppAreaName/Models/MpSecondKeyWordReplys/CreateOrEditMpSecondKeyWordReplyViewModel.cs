using Abp.AutoMapper;
using Pb.Wechat.MpSecondKeyWordReplys.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpSecondKeyWordReplys
{
    [AutoMapFrom(typeof(MpSecondKeyWordReplyDto))]
    public class CreateOrEditMpSecondKeyWordReplyViewModel : MpSecondKeyWordReplyDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpSecondKeyWordReplyViewModel(MpSecondKeyWordReplyDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpSecondKeyWordReplyViewModel()
        {
        }
    }
}
