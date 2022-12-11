using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Domain.Payers;
using Demo.BuildingBlocks;
using MediatR;

namespace Demo.Api.Payments.Application.Payers.GetPayer;

public sealed class GetPayerHandler : IRequestHandler<GetPayerQuery, PayerResponse>
{
    private readonly IPayersRepository _repository;

    public GetPayerHandler(IPayersRepository repository)
        => _repository = repository;

    public Task<PayerResponse> Handle(GetPayerQuery query, CancellationToken cancellationToken)
    {
        var customer = _repository.Get(query.id);
        if(customer is null)
        {
            throw new DomainException("Payer not found");
        }

        return Task.FromResult(new PayerResponse(
            customer.Id,
            customer.Name,
            customer.Address));
    }
}
