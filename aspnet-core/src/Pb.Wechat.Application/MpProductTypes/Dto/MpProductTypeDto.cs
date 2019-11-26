using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel;

namespace Pb.Wechat.MpProductTypes.Dto
{
    [AutoMap(typeof(MpProductType))]
    public class MpProductTypeDto : EntityDto
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string FilePathOrUrl { get; set; }
        public int SortIndex { get; set; }
    }
}
