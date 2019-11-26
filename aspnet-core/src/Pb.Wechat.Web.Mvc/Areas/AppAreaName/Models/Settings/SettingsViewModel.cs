using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pb.Wechat.Configuration.Tenants.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}