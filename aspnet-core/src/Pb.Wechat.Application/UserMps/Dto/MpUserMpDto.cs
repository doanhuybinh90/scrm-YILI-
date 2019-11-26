using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel;

namespace Pb.Wechat.UserMps.Dto
{
    [AutoMap(typeof(UserMp))]
    public class UserMpDto : EntityDto<int>
    {

/// <summary>
        /// MpID
        /// </summary>
        [Description("UserId")]
        public long? UserId { get; set; }
        /// <summary>
        /// MpID
        /// </summary>
        [Description("CurrentMpID")]
        public int CurrentMpID { get; set; }

        
    }
}
