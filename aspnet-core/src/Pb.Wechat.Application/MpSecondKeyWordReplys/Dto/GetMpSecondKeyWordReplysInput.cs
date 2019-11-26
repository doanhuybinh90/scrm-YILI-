using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;

namespace Pb.Wechat.MpSecondKeyWordReplys.Dto
{
    public class GetMpSecondKeyWordReplysInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Keyword { get; set; }
        public int? ParentId { get; set; }
        public MpMessageType? ReplyType { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id ASC";
            }
        }
    }
}
