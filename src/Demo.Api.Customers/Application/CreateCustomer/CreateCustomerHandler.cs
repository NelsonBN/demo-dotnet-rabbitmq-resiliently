using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Customers.Domain;
using Demo.BuildingBlocks;
using MediatR;

namespace Demo.Api.Customers.Application.CreateCustomer;

public sealed class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    private readonly ICustomersRepository _repository;
    private readonly IMessageBusDispatcher _dispatcher;

    public CreateCustomerHandler(ICustomersRepository repository, IMessageBusDispatcher dispatcher)
    {
        _repository = repository;
        _dispatcher = dispatcher;
    }

    public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        if(_repository.Any(command.Name))
        {
            throw new DomainException("A customer with the same name already exists");
        }

        var customer = Customer
            .Create(command.Name, command.Address);

        _repository.Add(customer);

        await _dispatcher.Publish(customer, cancellationToken);

        return new(customer.Id);
    }
}
