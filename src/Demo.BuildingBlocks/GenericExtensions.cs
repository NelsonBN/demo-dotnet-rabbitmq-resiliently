using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.BuildingBlocks;

public static class GenericExtensions
{
    public static async Task Publish(this IMessageBusDispatcher dispatcher, Entity entity, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        foreach(var domainEvent in entity.DomainEvents)
        {
            tasks.Add(dispatcher.Publish(domainEvent, cancellationToken));
        }

        await Task.WhenAll(tasks);

        entity.CleanDomainEvent();
    }

    public static string? GetMessageType(this DomainEvent? domainEvent)
        => domainEvent?
            .GetType()?
            .GetProperty(nameof(IMessage.MessageType), BindingFlags.Static | BindingFlags.Public)?
            .GetValue(domainEvent, null) as string;
}
