using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Customers.Domain;
using Demo.BuildingBlocks;
using MediatR;

namespace Demo.Api.Customers.Application.UpdateCustomerPhoto;

public sealed class UpdateCustomerPhotoHandler : IRequestHandler<UpdateCustomerPhotoCommand>
{
    private readonly ICustomersRepository _repository;
    private readonly IMessageBusDispatcher _dispatcher;

    public UpdateCustomerPhotoHandler(ICustomersRepository repository, IMessageBusDispatcher dispatcher)
    {
        _repository = repository;
        _dispatcher = dispatcher;
    }

    public async Task<Unit> Handle(UpdateCustomerPhotoCommand command, CancellationToken cancellationToken)
    {
        var customer = _repository.Get(command.Id);
        if(customer is null)
        {
            throw new DomainException("Customer not found");
        }

        customer.UpdatePhoto(command.PhotoUrl);

        await _dispatcher.Publish(customer, cancellationToken);

        return Unit.Value;
    }
}
