using Abp.AutoMapper;
using Pb.Wechat.MultiTenancy;
using Pb.Wechat.MultiTenancy.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.Common;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }

        public TenantFeaturesEditViewModel(Tenant tenant, GetTenantFeaturesEditOutput output)
        {
            Tenant = tenant;
            output.MapTo(this);
        }
    }
}