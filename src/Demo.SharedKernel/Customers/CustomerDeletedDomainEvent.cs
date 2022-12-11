using Demo.BuildingBlocks;

namespace Demo.SharedKernel.Customers;

public sealed record CustomerDeletedDomainEvent : DomainEvent, IMessage
{
    public static string MessageType => "Domain.Customers.CustomerDeleted";
}
