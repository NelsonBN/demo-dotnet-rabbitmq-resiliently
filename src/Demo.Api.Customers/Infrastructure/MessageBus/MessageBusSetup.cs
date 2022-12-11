using Demo.BuildingBlocks;
using Demo.BuildingBlocks.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Api.Customers.Infrastructure.MessageBus;

public static class MessageBusSetup
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<MessageBusConfig>(configuration.GetSection("MessageBus"))
            .SetupMessageBus(configuration)
            .AddScoped<IMessageBusDispatcher, MessageBusDispatcher>();

        services.AddHostedService<MessageBusSetupWorker>();

        return services;
    }
}
