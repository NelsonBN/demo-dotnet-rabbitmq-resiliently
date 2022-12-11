using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.BuildingBlocks;
using Demo.BuildingBlocks.MessageBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Demo.Api.Customers.Infrastructure.MessageBus;

internal sealed class MessageBusDispatcher : IMessageBusDispatcher
{
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);

    private readonly IOptions<MessageBusConfig> _config;
    private readonly ILogger<MessageBusDispatcher> _logger;

    private readonly IMessageBus _messageBus;
    private IModel? _channel;

    public MessageBusDispatcher(
        IOptions<MessageBusConfig> config,
        ILogger<MessageBusDispatcher> logger,
        IMessageBus messageBus
    )
    {
        _config = config;
        _logger = logger;
        _messageBus = messageBus;
    }

    public Task<string> Publish<TMessage>(TMessage domainEvent, CancellationToken cancellationToken = default)
        where TMessage : DomainEvent
    {
        if(domainEvent is null)
        {
            throw new ArgumentNullException(nameof(domainEvent));
        }

        var messageType = domainEvent.GetMessageType();
        if(messageType is null)
        {
            throw new ArgumentNullException($"{nameof(domainEvent)} > {nameof(messageType)}", "Invalid message type");
        }

        var channel = _getChannel(cancellationToken);

        var properties = _createProperties(channel, messageType!);

        channel.BasicPublish(
            exchange: _config.Value.ExchangeDomain,
            routingKey: messageType,
            basicProperties: properties,
            body: domainEvent.Serialize());

        // https://www.rabbitmq.com/confirms.html#publisher-confirms
        channel.WaitForConfirmsOrDie(_timeout);

        _logger.LogInformation($"[MESSAGE BUS][DISPATCHER] Message sent -> payload {domainEvent}");

        return Task.FromResult(properties.MessageId);
    }


    private IModel _getChannel(CancellationToken cancellationToken)
    {
        if(_channel is null)
        {
            _channel = createChannel();
        }
        else if(_channel.IsClosed)
        {
            _channel.Dispose();
            _channel = createChannel();
        }

        return _channel;

        IModel createChannel()
        {
            var channel = _messageBus.CreateChannel(3, cancellationToken);
            channel.ConfirmSelect();

            return channel;
        }
    }


    private static IBasicProperties _createProperties(IModel channel, string messageType)
        => channel.CreateBasicProperties()
                  .SetAppId()
                  .SetCorrelationId()
                  .SetMessageId()
                  .SetAsDurable()
                  .SetContentTypeJson()
                  .SetEncodingUTF8()
                  .SetMessageType(messageType);
}
