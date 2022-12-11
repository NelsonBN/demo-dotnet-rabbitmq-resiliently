using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Domain.Payers;
using Demo.SharedKernel.Customers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Api.Payments.Application.Payers.EventHendlers;

public sealed class PayerCreatedDomainEventHandler : INotificationHandler<CustomerCreatedDomainEvent>
{
    private readonly ILogger<PayerCreatedDomainEventHandler> _logger;
    private readonly IPayersRepository _repository;

    public PayerCreatedDomainEventHandler(ILogger<PayerCreatedDomainEventHandler> logger, IPayersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public Task Handle(CustomerCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var payer = _repository.Get(domainEvent.Id);

        if(payer is null)
        {
            payer = Payer.Create(domainEvent.Id, domainEvent.Name, domainEvent.Address);
            _repository.Add(payer);

            _logger.LogInformation($"[EVENT DOMAIN][PAYER][CREATED] Created -> Id: {payer.Id}, Name: {payer.Name}");

            return Task.CompletedTask;
        }

        payer.Update(domainEvent.Name, domainEvent.Address);

        _logger.LogInformation($"[EVENT DOMAIN][PAYER][CREATED] Updated -> Id: {payer.Id}, Name: {payer.Name}");

        return Task.CompletedTask;
    }
}
