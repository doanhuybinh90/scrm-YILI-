using Pb.Wechat.MpGroups.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpFans
{
    interface IMpGroupEditViewModel
    {
        List<MpGroupTreeDto> Groups { get; set; }

        List<string> GroupNames { get; set; }
    }

}
