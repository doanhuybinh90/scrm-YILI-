using Abp.AutoMapper;
using Pb.Wechat.MultiTenancy.Dto;

namespace Pb.Wechat.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}