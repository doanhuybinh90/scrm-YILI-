using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.MpMediaImageTypes.Dto
{
    [AutoMap(typeof(MpMediaImageType))]
    public class MpMediaImageTypeDto : EntityDto<int>
    {

        public int MpID { get; set; }

        public string MediaTypeName { get; set; }
               
        public DateTime? LastModificationTime { get; set; }
        public DateTime? CreationTime { get; set; }
    }
}
