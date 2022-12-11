namespace Demo.Api.Payments.Infrastructure.MessageBus;

public sealed class MessageBusConfig
{
    public string ExchangeDomain { get; init; } = default!;
    public string QueuePayments { get; set; } = default!;
}
