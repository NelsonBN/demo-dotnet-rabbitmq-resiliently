using System;
using Demo.BuildingBlocks;

namespace Demo.Api.Payments.Domain.Payers;

public sealed class Payer : Entity
{
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;

    private Payer(Guid id) : base(id) { }

    public void Update(string name, string address)
    {
        Name = name;
        Address = address;
    }

    public static Payer Create(Guid id, string name, string address)
        => new(id)
        {
            Name = name,
            Address = address
        };
}
