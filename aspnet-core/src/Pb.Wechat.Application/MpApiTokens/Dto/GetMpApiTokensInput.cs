using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using System;

namespace Pb.Wechat.MpApiTokens.Dto
{
    public class GetMpApiTokensInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int ParentId { get; set; }
        public string TokenName { get; set; }
        public MpApiTokenType? TokenType { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
