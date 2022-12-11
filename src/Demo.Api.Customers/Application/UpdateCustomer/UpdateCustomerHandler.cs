using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Customers.Domain;
using Demo.BuildingBlocks;
using MediatR;

namespace Demo.Api.Customers.Application.UpdateCustomer;

public sealed class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly ICustomersRepository _repository;
    private readonly IMessageBusDispatcher _dispatcher;

    public UpdateCustomerHandler(ICustomersRepository repository, IMessageBusDispatcher dispatcher)
    {
        _repository = repository;
        _dispatcher = dispatcher;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        if(_repository.Any(command.Name, command.Id))
        {
            throw new DomainException("A customer with the same name already exists");
        }

        var customer = _repository.Get(command.Id);
        if(customer is null)
        {
            throw new DomainException("Customer not found");
        }

        customer.Update(command.Name, command.Address);

        await _dispatcher.Publish(customer, cancellationToken);

        return Unit.Value;
    }
}
