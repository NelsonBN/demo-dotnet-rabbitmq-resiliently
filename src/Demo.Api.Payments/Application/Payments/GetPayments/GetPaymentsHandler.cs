using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Domain.Payments;
using MediatR;

namespace Demo.Api.Payments.Application.Payments.GetPayments;

public sealed class GetPaymentsHandler : IRequestHandler<GetPaymentsQuery, IEnumerable<PaymentSummaryResponse>>
{
    private readonly IPaymentsRepository _repository;

    public GetPaymentsHandler(IPaymentsRepository repository)
        => _repository = repository;

    public Task<IEnumerable<PaymentSummaryResponse>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = _repository
            .List()
            .Select(s => new PaymentSummaryResponse(
                s.Id,
                s.PayerName,
                s.Amount));

        return Task.FromResult(payments);
    }
}
