using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.BuildingBlocks;
using Demo.BuildingBlocks.MessageBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Demo.Api.Payments.Infrastructure.MessageBus;

internal sealed class MessageBusConsumer : IWorker
{
    private static readonly Dictionary<string, Type> _supportedMessages = new();

    private readonly IOptions<MessageBusConfig> _config;
    private readonly ILogger<MessageBusConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public MessageBusConsumer(
        IOptions<MessageBusConfig> config,
        ILogger<MessageBusConsumer> logger,
        IServiceProvider serviceProvider
    )
    {
        _config = config;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }



    public static void Assign<TMessage>()
        where TMessage : IMessage
        => _supportedMessages.Add(TMessage.MessageType, typeof(TMessage));


    public string Setup(IModel channel)
    {
        channel.SubscribeExchangeWithDeadletter(
                queueName: _config.Value.QueuePayments,
                exchangeName: _config.Value.ExchangeDomain,
                routes: _supportedMessages.Keys.ToArray());

        return _config.Value.QueuePayments;
    }


    public async Task Listener(IModel channel, BasicDeliverEventArgs args)
    {
        try
        {
            await _handle(_deserialize(args));

            channel.BasicAck(
                args.DeliveryTag,
                false);
        }
        catch(NotSupportedException exception)
        {
            _logger.LogError(
               exception,
               $"[MESSAGE BUS][CONSUMER][DESERIALIZE]"
            );
            channel.BasicReject(
                args.DeliveryTag,
                false);
        }
        catch(Exception exception)
        {
            _logger.LogError(
                exception,
                $"[MESSAGE BUS][CONSUMER][HANDLE]"
            );

            channel.BasicNack(
                args.DeliveryTag,
                false,
                false);
        }
    }

    private async Task _handle(IMessage message)
    {
        using(var scope = _serviceProvider.CreateScope())
        {
            await scope.ServiceProvider
                .GetRequiredService<IMediator>()
                .Publish(message);
        }
    }

    private static IMessage _deserialize(BasicDeliverEventArgs args)
    {
        var messageType = args.GetMessageType();

        if(!_supportedMessages.TryGetValue(messageType, out var type))
        {
            throw new NotSupportedException($"The message type '{messageType}' is not supported by consummer");
        }

        if(args.Deserialize(type) is not IMessage message)
        {
            throw new NotSupportedException($"The message type '{messageType}' is null");
        }

        return message;
    }
}
