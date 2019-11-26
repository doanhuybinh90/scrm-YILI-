using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.MpSelfArticleGroups.Dto
{
    [AutoMap(typeof(MpSelfArticleGroup))]
    public class MpSelfArticleGroupDto : EntityDto<int>
    {

        [Description("MpID")]
        public int MpID { get; set; }


        [Description("Name")]
        public string Name { get; set; }

        public string ItemIds { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
