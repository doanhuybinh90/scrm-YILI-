using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.CustomerServiceResponseTypes.Dto
{
    [AutoMap(typeof(CustomerServiceResponseType))]
    public class CustomerServiceResponseTypeDto : EntityDto<int>
    {
        public string TypeDescription { get; set; }
       
        public DateTime? LastModificationTime { get; set; }
       
    }
}
