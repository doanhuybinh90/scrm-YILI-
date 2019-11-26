using System.Threading.Tasks;
using Abp.Application.Services;
using Pb.Wechat.MultiTenancy.Dto;
using Pb.Wechat.MultiTenancy.Payments.Dto;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.MultiTenancy.Payments
{
    public interface IPaymentAppService : IApplicationService
    {
        Task<PaymentInfoDto> GetPaymentInfo(PaymentInfoInput input);

        Task<CreatePaymentResponse> CreatePayment(CreatePaymentDto input);

        Task<ExecutePaymentResponse> ExecutePayment(ExecutePaymentDto input);

        Task<PagedResultDto<SubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input);
    }
}
