using System.Collections.Generic;
using Pb.Wechat.Caching.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}