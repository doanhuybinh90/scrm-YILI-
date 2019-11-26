using Abp.AutoMapper;
using Pb.Wechat.MpFansTags.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpFansTags
{
    [AutoMapFrom(typeof(MpFansTagDto))]
    public class CreateOrEditMpFansTagViewModel : MpFansTagDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpFansTagViewModel(MpFansTagDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpFansTagViewModel()
        {
        }
    }
}
