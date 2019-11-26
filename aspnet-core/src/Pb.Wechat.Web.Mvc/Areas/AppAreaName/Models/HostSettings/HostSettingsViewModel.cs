using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pb.Wechat.Configuration.Host.Dto;
using Pb.Wechat.Editions.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}