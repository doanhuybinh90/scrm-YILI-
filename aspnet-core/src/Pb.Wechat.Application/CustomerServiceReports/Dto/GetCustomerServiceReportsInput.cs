using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerServiceReports.Dto
{
    public class GetCustomerServiceReportsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public DateTime? StatistStartDate { get; set; }
        public DateTime? StatistEndDate { get; set; }
        public string NickName { get; set; }
        public string NeedSum { get; set; }
        //public List<int> CustomerIds { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "ReportDate DESC,NickName Asc";
            }
        }
    }
}
