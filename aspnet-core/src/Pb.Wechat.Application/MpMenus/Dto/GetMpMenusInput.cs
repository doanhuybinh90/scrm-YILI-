using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;

namespace Pb.Wechat.MpMenus.Dto
{
    public class GetMpMenusInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public int Id { get; set; }
        public MpMenuType? Type { get; set; }
        public MpMessageType? MediaType { get; set; }
        public string MenuKey { get; set; }
        public int NewParentId { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id ASC";
            }
        }
    }
}
