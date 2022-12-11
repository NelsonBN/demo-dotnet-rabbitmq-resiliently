using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Domain.Payments;
using Demo.BuildingBlocks;
using MediatR;

namespace Demo.Api.Payments.Application.Payments.GetPayment;

public sealed class GetPaymentHandler : IRequestHandler<GetPaymentQuery, PaymentResponse>
{
    private readonly IPaymentsRepository _repository;

    public GetPaymentHandler(IPaymentsRepository repository)
        => _repository = repository;

    public Task<PaymentResponse> Handle(GetPaymentQuery query, CancellationToken cancellationToken)
    {
        var payment = _repository.Get(query.id);
        if(payment is null)
        {
            throw new DomainException("Payment not found");
        }

        return Task.FromResult(new PaymentResponse(
            payment.Id,
            payment.PayerId,
            payment.PayerName,
            payment.Amount,
            payment.CreatedAt));
    }
}
