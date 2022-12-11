using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Domain.Payers;
using Demo.SharedKernel.Customers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Api.Payments.Application.Payers.EventHendlers;

public sealed class PayerDeletedDomainEventHandler : INotificationHandler<CustomerDeletedDomainEvent>
{
    private readonly ILogger<PayerDeletedDomainEventHandler> _logger;
    private readonly IPayersRepository _repository;

    public PayerDeletedDomainEventHandler(ILogger<PayerDeletedDomainEventHandler> logger, IPayersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public Task Handle(CustomerDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        if(!_repository.Any(domainEvent.Id))
        {
            return Task.CompletedTask;
        }

        _repository.Delete(domainEvent.Id);

        _logger.LogInformation($"[EVENT DOMAIN][PAYER][DELETED] Id: {domainEvent.Id}");

        return Task.CompletedTask;
    }
}
