using Abp.AutoMapper;
using Pb.Wechat.Sessions.Dto;

namespace Pb.Wechat.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}