using Demo.BuildingBlocks;

namespace Demo.SharedKernel.Customers;

public sealed record CustomerCreatedDomainEvent : DomainEvent, IMessage
{
    public static string MessageType => "Domain.Customers.CustomerCreated";

    public required string Name { get; init; }
    public string Address { get; init; } = default!;
}
