using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.MpAccounts.Dto
{
    [AutoMap(typeof(MpAccount))]
    public class MpAccountDto : EntityDto<int>
    {
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "类型不能为空")]
        [StringLength(50)]
        public string AccountType { get; set; }
        [Required(ErrorMessage = "AppId不能为空")]
        [StringLength(200)]
        public string AppId { get; set; }
        [Required(ErrorMessage = "AppSecret不能为空")]
        [StringLength(500)]
        public string AppSecret { get; set; }
        [StringLength(200)]
        public string Token { get; set; }
        [StringLength(500)]
        public string EncodingAESKey { get; set; }
        [StringLength(200)]
        public string MchID { get; set; }
        [StringLength(500)]
        public string WxPayAppSecret { get; set; }
        [StringLength(500)]
        public string CertPhysicalPath { get; set; }
        [StringLength(50)]
        public string CertPassword { get; set; }
        public string Remark { get; set; }
        public string TaskAccessToken { get; set; }
        public string WxAccount { get; set; }
    }
}
