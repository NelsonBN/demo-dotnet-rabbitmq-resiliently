using System;
using Demo.Api.Payments.Domain.Payers;
using Demo.BuildingBlocks;

namespace Demo.Api.Payments.Domain.Payments;

public sealed class Payment : Entity
{
    public Guid PayerId { get; init; }
    public string PayerName { get; init; } = default!;
    public double Amount { get; init; }
    public DateTime CreatedAt { get; init; }

    public Payment() : base() { }


    public static Payment Create(Payer payer, double amount)
        => new()
        {
            PayerId = payer.Id,
            PayerName = payer.Name,
            Amount = amount,
            CreatedAt = DateTime.UtcNow
        };
}
