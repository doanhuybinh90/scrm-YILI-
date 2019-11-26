using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace Pb.Wechat.MpGroups.Dto
{
    [AutoMap(typeof(MpGroup))]
    public class MpGroupDto : EntityDto<int>
    {

        public int MpID { get; set; }

        public string Name { get; set; }
        public int FansCount { get; set; }
        public int ParentID { get; set; }
        public string FullPath { get; set; }
        public int Length { get; set; }
        public int ChildCount { get; set; }
        public int WxGroupID { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string ParentName { get; set; }
    }
}
