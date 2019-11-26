using Abp.AutoMapper;
using Pb.Wechat.MpChannels.Dto;
using Pb.Wechat.MpFansTags.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpChannels
{
    [AutoMapFrom(typeof(MpChannelDto))]
    public class CreateOrEditMpChannelViewModel : MpChannelDto
    {
        public bool IsEditMode => Id!=0;

        public IList<MpFansTagDto> Tags = null;
        public CreateOrEditMpChannelViewModel(MpChannelDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpChannelViewModel() {
        }
    }
}
