using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Customers.Domain;
using Demo.BuildingBlocks;
using MediatR;

namespace Demo.Api.Customers.Application.DeleteCustomer;

public sealed class DeleteCustomerHanldler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomersRepository _repository;
    private readonly IMessageBusDispatcher _dispatcher;

    public DeleteCustomerHanldler(ICustomersRepository repository, IMessageBusDispatcher dispatcher)
    {
        _repository = repository;
        _dispatcher = dispatcher;
    }

    public async Task<Unit> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = _repository.Get(command.Id);
        if(customer is null)
        {
            throw new DomainException("Customer not found");
        }

        customer.Delete();

        _repository.Delete(customer);

        await _dispatcher.Publish(customer, cancellationToken);

        return Unit.Value;
    }
}
