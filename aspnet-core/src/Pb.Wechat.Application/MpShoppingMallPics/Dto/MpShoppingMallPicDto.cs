using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.MpShoppingMallPics.Dto
{
    [AutoMap(typeof(MpShoppingMallPic))]
    public class MpShoppingMallPicDto : EntityDto<int>
    {
        public int MpID { get; set; }
        public string Name { get; set; }
        public string LocalPicUrl { get; set; }
        public string LocalPicPath { get; set; }
        public string LinkUrl { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
