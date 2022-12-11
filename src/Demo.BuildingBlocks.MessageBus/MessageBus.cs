using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Demo.BuildingBlocks.MessageBus;

// https://stackoverflow.com/questions/12499174/rabbitmq-c-sharp-driver-stops-receiving-messages/12735286#12735286
// https://gigi.nullneuron.net/gigilabs/resilient-connections-with-rabbitmq-net-client/
// https://github.com/rabbitmq/rabbitmq-dotnet-client/issues/1061
public interface IMessageBus
{
    IModel CreateChannel(CancellationToken cancellationToken = default);
    IModel CreateChannel(int maxRetries, CancellationToken cancellationToken = default);
    IModel CreateChannel(int maxWaitRetrySeconds, int? maxRetries = null, CancellationToken cancellationToken = default);
}

internal sealed class MessageBus : IMessageBus, IDisposable
{
    private readonly ILogger<MessageBus> _logger;
    private readonly IConnectionFactory _factory;

    private IConnection? _connection;

    public MessageBus(
        ILogger<MessageBus> logger,
        ConnectionFactory factory
    )
    {
        _logger = logger;
        _factory = factory;

        _connection = default!;
    }

    public IModel CreateChannel(CancellationToken cancellationToken = default)
        => CreateChannel(Defaults.MAX_WAIT_RETRY_SECONDS, null, cancellationToken);

    public IModel CreateChannel(int maxAttempts, CancellationToken cancellationToken = default)
        => CreateChannel(Defaults.MAX_WAIT_RETRY_SECONDS, maxAttempts, cancellationToken);

    public IModel CreateChannel(int maxWaitRetrySeconds, int? maxAttempts = null, CancellationToken cancellationToken = default)
        => _getConnection(maxWaitRetrySeconds, maxAttempts, cancellationToken).CreateModel();


    private IConnection _getConnection(int maxWaitRetrySeconds, int? maxAttempts, CancellationToken cancellationToken = default)
    {
        if(_connection is null)
        {
            _connection = createConnection();
        }

        else if(!_connection.IsOpen)
        {
            Dispose();
            _connection = createConnection();
        }

        return _connection;

        IConnection createConnection()
        {
            IConnection connection = default!;

            _buildPolicy(maxWaitRetrySeconds, maxAttempts)
                .Execute(_ => connection = _factory.CreateConnection(), cancellationToken);

            return connection;
        }
    }

    private RetryPolicy _buildPolicy(int maxWaitRetrySeconds, int? maxAttempts)
    {
        var policy = Policy.Handle<BrokerUnreachableException>();

        if(maxAttempts is null)
        {
            return policy.WaitAndRetryForever(sleepDurationProvider);
        }

        return policy.WaitAndRetry(maxAttempts.Value - 1, sleepDurationProvider);

        TimeSpan sleepDurationProvider(int attempt)
        {
            var seconds = Math.Min(Math.Pow(2, attempt), maxWaitRetrySeconds);
            var timeToWait = TimeSpan.FromSeconds(seconds);

            _logger.LogError($"[MESSAGE BUS][CONNECTION] Attempt {attempt}{(maxAttempts is null ? "" : $"/{maxAttempts}")} failed to connect. Will try again on '{timeToWait}'");

            return timeToWait;
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
        _connection = null;
    }
}
