using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace Pb.Wechat.CYProblems.Dto
{
    [AutoMap(typeof(CYProblem))]
    public class CYProblemDto : EntityDto
    {
        public int FansId { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string Telephone { get; set; }
        public string UserName { get; set; }
        public string BabyName { get; set; }
        public int? CYProblemId { get; set; }
        public int State { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool CloseNotice { get; set; }
    }
}
