using Abp.AutoMapper;
using Pb.Wechat.CustomerServiceConversations.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceConversations
{
    [AutoMapFrom(typeof(CustomerServiceConversationDto))]
    public class CreateOrEditCustomerServiceConversationViewModel : CustomerServiceConversationDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCustomerServiceConversationViewModel(CustomerServiceConversationDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerServiceConversationViewModel()
        {
        }
    }
}
