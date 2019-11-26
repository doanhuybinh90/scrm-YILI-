using Abp.AutoMapper;
using Pb.Wechat.CustomerServiceConversationMsgs.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceConversationMsgs
{
    [AutoMapFrom(typeof(CustomerServiceConversationMsgDto))]
    public class CreateOrEditCustomerServiceConversationMsgViewModel : CustomerServiceConversationMsgDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCustomerServiceConversationMsgViewModel(CustomerServiceConversationMsgDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerServiceConversationMsgViewModel()
        {
        }
    }
}
