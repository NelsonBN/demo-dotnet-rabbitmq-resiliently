using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Customers.Domain;
using MediatR;

namespace Demo.Api.Customers.Application.GetCustomers;

public sealed class GetCustomersHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerSummaryResponse>>
{
    private readonly ICustomersRepository _repository;

    public GetCustomersHandler(ICustomersRepository repository)
        => _repository = repository;

    public Task<IEnumerable<CustomerSummaryResponse>> Handle(GetCustomersQuery query, CancellationToken cancellationToken)
    {
        var customers = _repository
            .List()
            .Select(s => new CustomerSummaryResponse(
                s.Id, s.Name
            ));

        return Task.FromResult(customers);
    }
}
