using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace Pb.Wechat.CYProblemContents
{
    public class CYProblemContent : Entity<int>, IHasCreationTime
    {
        public int ProblemId { get; set; }
        /// <summary>
        /// 0用户提问，1医生回答
        /// </summary>
        public int SendUser { get; set; }
        /// <summary>
        /// text文本，image图片url，audio音频url，patient_meta病人资料
        /// </summary>
        public int Type { get; set; }
        public string Text { get; set; }
        public string File { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public DateTime CreationTime { get; set; }
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; }
    }
}
