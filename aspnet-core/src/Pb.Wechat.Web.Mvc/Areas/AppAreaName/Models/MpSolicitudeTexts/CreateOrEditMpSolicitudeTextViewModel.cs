using Abp.AutoMapper;
using Pb.Wechat.MpSolicitudeTexts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpSolicitudeTexts
{
    [AutoMapFrom(typeof(MpSolicitudeTextDto))]
    public class CreateOrEditMpSolicitudeTextViewModel : MpSolicitudeTextDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpSolicitudeTextViewModel(MpSolicitudeTextDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpSolicitudeTextViewModel()
        {
        }
    }
}
