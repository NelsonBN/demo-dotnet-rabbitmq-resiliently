using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Customers.Domain;
using Demo.BuildingBlocks;
using MediatR;

namespace Demo.Api.Customers.Application.GetCustomer;

public sealed class GetCustomerHandler : IRequestHandler<GetCustomerQuery, CustomerResponse>
{
    private readonly ICustomersRepository _repository;

    public GetCustomerHandler(ICustomersRepository repository)
        => _repository = repository;

    public Task<CustomerResponse> Handle(GetCustomerQuery query, CancellationToken cancellationToken)
    {
        var customer = _repository.Get(query.Id);
        if(customer is null)
        {
            throw new DomainException("Customer not found");
        }

        return Task.FromResult(new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Address,
            customer.PhotoUrl));
    }
}
