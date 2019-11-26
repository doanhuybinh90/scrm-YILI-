using Abp.AutoMapper;
using Pb.Wechat.Editions;
using Pb.Wechat.MultiTenancy.Payments.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.SubscriptionManagement
{
    [AutoMapTo(typeof(ExecutePaymentDto))]
    public class PaymentResultViewModel : SubscriptionPaymentDto
    {
        public EditionPaymentType EditionPaymentType { get; set; }
    }
}