using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Domain.Payers;
using MediatR;

namespace Demo.Api.Payments.Application.Payers.GetPayers;

public sealed class GetPayersHandler : IRequestHandler<GetPayersQuery, IEnumerable<PayerSummaryResponse>>
{
    private readonly IPayersRepository _repository;

    public GetPayersHandler(IPayersRepository repository)
        => _repository = repository;

    public Task<IEnumerable<PayerSummaryResponse>> Handle(GetPayersQuery query, CancellationToken cancellationToken)
    {
        var payers = _repository
            .List()
            .Select(s => new PayerSummaryResponse(
                s.Id, s.Name
            ));

        return Task.FromResult(payers);
    }
}
