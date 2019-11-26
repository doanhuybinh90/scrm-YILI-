using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace Pb.Wechat.CYProblems
{
    public class CYProblem : Entity<int>, IHasCreationTime, IHasModificationTime
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
