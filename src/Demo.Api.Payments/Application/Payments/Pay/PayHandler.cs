using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Domain.Payers;
using Demo.Api.Payments.Domain.Payments;
using Demo.BuildingBlocks;
using MediatR;

namespace Demo.Api.Payments.Application.Payments.Pay;

public sealed class PayHandler : IRequestHandler<PayCommand, PayResponse>
{
    private readonly IPaymentsRepository _paymentsRepository;
    private readonly IPayersRepository _payersRepository;

    public PayHandler(IPaymentsRepository paymentsRepository, IPayersRepository payersRepository)
    {
        _paymentsRepository = paymentsRepository;
        _payersRepository = payersRepository;
    }

    public Task<PayResponse> Handle(PayCommand command, CancellationToken cancellationToken)
    {
        if(command.Amount <= 0)
        {
            throw new DomainException("The amount must be greater than zero");
        }

        var payer = _payersRepository.Get(command.PayerId);
        if(payer is null)
        {
            throw new DomainException("Payer not found");
        }

        var payment = Payment.Create(payer, command.Amount);

        _paymentsRepository.Add(payment);

        return Task.FromResult(new PayResponse(payment.Id));
    }
}
