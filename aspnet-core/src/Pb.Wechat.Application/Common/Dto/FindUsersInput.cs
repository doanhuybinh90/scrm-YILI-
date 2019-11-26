using Pb.Wechat.Dto;

namespace Pb.Wechat.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}