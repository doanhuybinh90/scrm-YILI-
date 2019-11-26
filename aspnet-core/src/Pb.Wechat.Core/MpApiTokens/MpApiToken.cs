using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpApiTokens
{
    public class MpApiToken : Entity, IAudited, ISoftDelete
    {

        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "公众号不能为空")]
        public int ParentId { get; set; }
        [Required(ErrorMessage = "令牌不能为空")]
        [StringLength(200)]
        public string Token { get; set; }
        [Required(ErrorMessage = "令牌类型不能为空")]
        [StringLength(50)]
        public string ApiType { get; set; }
        [Required(ErrorMessage = "域名不能为空")]
        [StringLength(50)]
        public string Domain { get; set; }
        [Required(ErrorMessage = "开始时间不能为空")]
        public DateTime StartTime { get; set; }
        //[Required(ErrorMessage = "结束时间不能为空")]
        public DateTime? EndTime { get; set; }


        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
