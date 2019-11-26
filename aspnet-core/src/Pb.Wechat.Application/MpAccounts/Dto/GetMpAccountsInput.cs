using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpAccounts.Dto
{
    public class GetMpAccountsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string AccountName { get; set; }
        public MpAccountType? AccountType { get; set; }
        public string Remark { get; set; }
        public string AppId { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
