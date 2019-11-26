using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pb.Wechat.MpFansTagItems
{
    public class MpFansTagItem : Entity<long>, IHasCreationTime, ISoftDelete
    {
        [Required(ErrorMessage = "公众号ID不能为空")]
        public int MpID { get; set; }
        [Required(ErrorMessage = "粉丝ID不能为空")]
        public int FansId { get; set; }
        [Required(ErrorMessage = "标签ID不能为空")]
        public int TagId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
