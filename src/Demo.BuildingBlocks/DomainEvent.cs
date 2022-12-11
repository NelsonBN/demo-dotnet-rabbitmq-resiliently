using System;

namespace Demo.BuildingBlocks;

public abstract record DomainEvent
{
    public required Guid Id { get; set; }
    public DateTime OccurredAt { get; private init; }

    protected DomainEvent()
        => OccurredAt = DateTime.UtcNow;
}
