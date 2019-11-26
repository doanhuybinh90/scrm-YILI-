using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.MpCourseSignups.Dto
{
    [AutoMap(typeof(MpCourseSignup))]
    public class MpCourseSignupDto : EntityDto<int>
    {
        public int MpID { get; set; }
        public string OpenID { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Address { get; set; }
        public bool IsConfirmed { get; set; }
        public bool SendMessageState { get; set; }
        public DateTime? SendTime { get; set; }
        public string SendResult { get; set; }
        public string Reamrk { get; set; }
        public int CRMID { get; set; }
    }
}
