using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Domain.Payers;
using Demo.SharedKernel.Customers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Api.Payments.Application.Payers.EventHendlers;

public sealed class PayerUpdatedDomainEventHandler : INotificationHandler<CustomerUpdatedDomainEvent>
{
    private readonly ILogger<PayerUpdatedDomainEventHandler> _logger;
    private readonly IPayersRepository _repository;

    public PayerUpdatedDomainEventHandler(ILogger<PayerUpdatedDomainEventHandler> logger, IPayersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public Task Handle(CustomerUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var payer = _repository.Get(domainEvent.Id);

        if(payer is null)
        {
            payer = Payer.Create(domainEvent.Id, domainEvent.Name, domainEvent.Address);
            _repository.Add(payer);

            _logger.LogInformation($"[EVENT DOMAIN][PAYER][UPDATED] Created -> Id: {payer.Id}, Name: {payer.Name}");

            return Task.CompletedTask;
        }

        payer.Update(domainEvent.Name, domainEvent.Address);

        _logger.LogInformation($"[EVENT DOMAIN][PAYER][UPDATED] Updated -> Id: {payer.Id}, Name: {payer.Name}");

        return Task.CompletedTask;
    }
}
