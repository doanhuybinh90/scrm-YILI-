using Pb.Wechat.Editions;
using Pb.Wechat.Editions.Dto;
using Pb.Wechat.Security;
using Pb.Wechat.MultiTenancy.Payments;
using Pb.Wechat.MultiTenancy.Payments.Dto;

namespace Pb.Wechat.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType? Gateway { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public bool ShowPaymentExpireNotification()
        {
            return !string.IsNullOrEmpty(PaymentId);
        }
    }
}
