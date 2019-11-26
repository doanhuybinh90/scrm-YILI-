using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.CustomerArticleGroups.Dto
{
    [AutoMap(typeof(CustomerArticleGroup))]
    public class CustomerArticleGroupDto : EntityDto<int>
    {

        [Description("MpID")]
        public int MpID { get; set; }


        [Description("Name")]
        public string Name { get; set; }

        public string ItemIds { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string MediaID { get; set; }
    }
}
