using Demo.BuildingBlocks;

namespace Demo.SharedKernel.Customers;

public sealed record CustomerPhotoUpdatedDomainEvent : DomainEvent, IMessage
{
    public static string MessageType => "Domain.Customers.CustomerPhotoUpdated";

    public string PhotoUrl { get; init; } = default!;
}
