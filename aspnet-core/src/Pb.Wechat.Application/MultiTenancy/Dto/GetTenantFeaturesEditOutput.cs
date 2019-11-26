using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pb.Wechat.Editions.Dto;

namespace Pb.Wechat.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}