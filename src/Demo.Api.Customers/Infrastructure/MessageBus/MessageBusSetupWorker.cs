using System.Threading;
using System.Threading.Tasks;
using Demo.BuildingBlocks.MessageBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Demo.Api.Customers.Infrastructure.MessageBus;

internal sealed class MessageBusSetupWorker : BackgroundService
{
    private readonly IOptions<MessageBusConfig> _config;
    private readonly ILogger<MessageBusSetupWorker> _logger;
    private readonly IMessageBus _messageBus;

    public MessageBusSetupWorker(
        IOptions<MessageBusConfig> config,
        ILogger<MessageBusSetupWorker> logger,
        IMessageBus messageBus
    )
    {
        _config = config;
        _logger = logger;
        _messageBus = messageBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[MESSAGE BUS][SETUP][WORKER] Setup starting...");

        await Task.Factory.StartNew(() =>
        {
            var channel = _messageBus.CreateChannel(stoppingToken);
            channel.CreateUnroutedExchange(_config.Value.ExchangeDomain);
        }, stoppingToken);

        _logger.LogInformation("[MESSAGE BUS][SETUP][WORKER] Finished setup");
    }
}
