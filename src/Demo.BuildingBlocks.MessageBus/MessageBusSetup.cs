using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Demo.BuildingBlocks.MessageBus;

public static class MessageBusSetup
{
    public static IServiceCollection SetupMessageBus(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton(configuration.GetSection("MessageBus").Get<ConnectionFactory>()!)
            .AddSingleton<IMessageBus, MessageBus>();
}
