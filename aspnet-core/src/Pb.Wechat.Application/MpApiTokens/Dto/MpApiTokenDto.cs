using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.MpApiTokens.Dto
{
    [AutoMap(typeof(MpApiToken))]
    public class MpApiTokenDto : EntityDto
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

        public DateTime CreationTime { get; set; }
    }
}
