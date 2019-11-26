using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpGroups.Dto
{
    public class GetMpGroupsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int Id { get; set; }
        public int MpID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public int WxGroupID { get; set; }
        public int NewParentId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
            
        }
    }
}
