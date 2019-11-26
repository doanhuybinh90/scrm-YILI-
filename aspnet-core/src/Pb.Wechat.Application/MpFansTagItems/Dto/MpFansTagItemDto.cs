using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.MpFansTagItems.Dto
{
    [AutoMap(typeof(MpFansTagItem))]
    public class MpFansTagItemDto : EntityDto<long>
    {
        public int MpID { get; set; }
        public int FansId { get; set; }
        public int TagId { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
