using Abp.AutoMapper;
using Pb.Wechat.MpSolicitudeSettings.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpSolicitudeSettings
{
    [AutoMapFrom(typeof(MpSolicitudeSettingDto))]
    public class CreateOrEditMpSolicitudeSettingViewModel : MpSolicitudeSettingDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpSolicitudeSettingViewModel(MpSolicitudeSettingDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpSolicitudeSettingViewModel()
        {
        }
    }
}
