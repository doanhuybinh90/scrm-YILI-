using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.MpProductInfos.Dto
{
    [AutoMap(typeof(MpProductInfo))]
    public class MpProductInfoDto : EntityDto<int>
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string FilePathOrUrl { get; set; }
        public string ProductIntroduce { get; set; }
        public string ProductFormulations { get; set; }
        public int SortIndex { get; set; }
        public int TypeId { get; set; }
        public string TypeTitle { get; set; }
    }
}
