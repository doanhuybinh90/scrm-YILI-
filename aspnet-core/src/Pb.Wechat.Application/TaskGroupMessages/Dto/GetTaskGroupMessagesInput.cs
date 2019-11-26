using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.TaskGroupMessages.Dto
{
    public class GetTaskGroupMessagesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int? MpID { get; set; }
        public string TaskID { get; set; }
        public int? MessageId { get; set; }
        public int? TaskState { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id Asc";
            }
        }
    }
}
