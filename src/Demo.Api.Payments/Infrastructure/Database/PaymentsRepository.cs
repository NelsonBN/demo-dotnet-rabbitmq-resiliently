using System;
using System.Collections.Generic;
using Demo.Api.Payments.Domain.Payments;

namespace Demo.Api.Payments.Infrastructure.Database;

public sealed class PaymentsRepository : IPaymentsRepository
{
    private static readonly Dictionary<Guid, Payment> _database = new();


    public IEnumerable<Payment> List()
        => _database.Values;

    public Payment? Get(Guid id)
    {
        _database.TryGetValue(id, out var payment);
        return payment;
    }

    public void Add(Payment payment)
        => _database.Add(payment.Id, payment);
}
