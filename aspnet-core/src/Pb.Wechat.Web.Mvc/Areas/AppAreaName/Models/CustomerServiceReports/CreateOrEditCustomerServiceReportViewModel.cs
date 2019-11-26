using Abp.AutoMapper;
using Pb.Wechat.CustomerServiceReports.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceReports
{
    [AutoMapFrom(typeof(CustomerServiceReportDto))]
    public class CreateOrEditCustomerServiceReportViewModel : CustomerServiceReportDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCustomerServiceReportViewModel(CustomerServiceReportDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerServiceReportViewModel()
        {
        }
    }
}
