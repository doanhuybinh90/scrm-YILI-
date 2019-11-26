using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using System;

namespace Pb.Wechat.CustomerServiceConversations.Dto
{
    public class GetCustomerServiceConversationsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public string Keyword { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string KfNickName { get; set; }
        public string FanNickName { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime Desc";
            }
            
        }
    }
}
