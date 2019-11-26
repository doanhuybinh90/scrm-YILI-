using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpFansTags.Dto
{
    [AutoMap(typeof(MpFansTag))]
    public class MpFansTagDto : EntityDto<int>, IAudited, ISoftDelete
    {
        public int MpID { get; set; }
        public string Name { get; set; }
        public long FansCount { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
