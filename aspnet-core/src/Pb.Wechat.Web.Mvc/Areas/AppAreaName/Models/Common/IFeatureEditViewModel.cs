using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pb.Wechat.Editions.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}