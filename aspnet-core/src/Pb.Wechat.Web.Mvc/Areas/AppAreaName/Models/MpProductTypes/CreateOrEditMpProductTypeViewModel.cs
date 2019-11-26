using Abp.AutoMapper;
using Pb.Wechat.MpProductTypes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpProductTypes
{
    [AutoMapFrom(typeof(MpProductTypeDto))]
    public class CreateOrEditMpProductTypeViewModel : MpProductTypeDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpProductTypeViewModel(MpProductTypeDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpProductTypeViewModel()
        {
        }
    }
}
