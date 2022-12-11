using Demo.BuildingBlocks.MessageBus;
using Demo.SharedKernel.Customers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Api.Payments.Infrastructure.MessageBus;

public static class MessageBusSetup
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<MessageBusConfig>(configuration.GetSection("MessageBus"))
            .SetupMessageBus(configuration);

        MessageBusConsumer.Assign<CustomerCreatedDomainEvent>();
        MessageBusConsumer.Assign<CustomerUpdatedDomainEvent>();
        MessageBusConsumer.Assign<CustomerDeletedDomainEvent>();

        services
            .AddSingleton<MessageBusConsumer>()
            .AddHostedService<MessageBusConsumerWorker<MessageBusConsumer>>();

        return services;
    }
}
