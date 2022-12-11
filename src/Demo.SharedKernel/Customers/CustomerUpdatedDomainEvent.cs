using Demo.BuildingBlocks;

namespace Demo.SharedKernel.Customers;

public sealed record CustomerUpdatedDomainEvent : DomainEvent, IMessage
{
    public static string MessageType => "Domain.Customers.CustomerUpdated";

    public required string Name { get; init; }
    public string Address { get; init; } = default!;
}
