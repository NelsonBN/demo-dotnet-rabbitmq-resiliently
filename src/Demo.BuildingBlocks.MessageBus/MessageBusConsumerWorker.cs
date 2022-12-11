using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Demo.BuildingBlocks.MessageBus;

public interface IWorker
{
    public string Setup(IModel channel);
    public Task Listener(IModel channel, BasicDeliverEventArgs args);
}

public sealed class MessageBusConsumerWorker<TWorker> : BackgroundService
    where TWorker : IWorker
{
    private readonly ILogger<MessageBusConsumerWorker<TWorker>> _logger;
    private readonly IMessageBus _messageBus;

    private readonly TWorker _worker;
    private AsyncEventingBasicConsumer _consumer = default!;
    private IModel _channel = default!;
    private string _consumerTag = default!;

    public MessageBusConsumerWorker(
        ILogger<MessageBusConsumerWorker<TWorker>> logger,
        IMessageBus messageBus,
        TWorker worker
    )
    {
        _logger = logger;
        _messageBus = messageBus;

        _worker = worker;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _start(stoppingToken);

        // Keep worker running
        while(!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"[MESSAGE BUS][WORKER][CONSUMER] Worker running at: {DateTimeOffset.UtcNow} - Consumer live {_consumer?.IsRunning == true}");
            await Task.Delay(5000, stoppingToken);
        }

        _channel?.StopConsume(_consumerTag);
    }

    private void _start(CancellationToken stoppingToken = default)
    {
        _logger.LogInformation("[MESSAGE BUS][WORKER][CONSUMER] starting...");

        _channel = _messageBus.CreateChannel(stoppingToken);

        _channel.BasicQos(
            0,
            10, // Total message receive same time
            false); // [ false per consumer | true per channel ]

        var queue = _worker.Setup(_channel);

        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.Received += (_, arg) => _worker.Listener(_channel, arg);
        _consumer.ConsumerCancelled += (_, arg) => _consumerCancelled(arg, stoppingToken);

        _consumerTag = _channel.BasicConsume(
            queue: queue,
            autoAck: false,
            consumer: _consumer);

        _logger.LogInformation("[MESSAGE BUS][WORKER][CONSUMER] started");
    }

    private Task _consumerCancelled(ConsumerEventArgs @event, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogError($"[MESSAGE BUS][WORKER][CONSUMER] disconnected");

            _consumer.Received -= (_, arg) => _worker.Listener(channel: _channel, arg);
            _consumer.ConsumerCancelled -= (ct, arg) => _consumerCancelled(arg, (CancellationToken)ct);

            _channel?.StopConsume(_consumerTag);
        }
        catch(Exception exception)
        {
            _logger.LogError(exception, $"[MESSAGE BUS][WORKER][CONSUMER] disconnected");
        }

        _channel = default!;

        _start(cancellationToken);

        return Task.CompletedTask;
    }
}
