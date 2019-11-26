using Abp.AutoMapper;
using Pb.Wechat.MultiTenancy.Payments;

namespace Pb.Wechat.Sessions.Dto
{
    [AutoMapFrom(typeof(SubscriptionPayment))]
    public class SubscriptionPaymentInfoDto
    {
        public decimal Amount { get; set; }
    }
}