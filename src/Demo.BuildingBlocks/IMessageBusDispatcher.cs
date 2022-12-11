using System.Threading;
using System.Threading.Tasks;

namespace Demo.BuildingBlocks;

public interface IMessageBusDispatcher
{
    Task<string> Publish<TMessage>(TMessage domainEvent, CancellationToken cancellationToken = default)
       where TMessage : DomainEvent;
}
