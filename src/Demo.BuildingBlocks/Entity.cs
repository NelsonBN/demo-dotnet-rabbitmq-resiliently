using System;
using System.Collections.Generic;

namespace Demo.BuildingBlocks;

public abstract class Entity
{
    public Guid Id { get; private init; }

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();


    protected Entity() => Id = Guid.NewGuid();

    protected Entity(Guid id) => Id = id;

    protected void AddDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    internal void CleanDomainEvent()
        => _domainEvents.Clear();
}
