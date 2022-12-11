using System;
using System.Collections.Generic;
using Demo.Api.Payments.Domain.Payers;

namespace Demo.Api.Payments.Infrastructure.Database;

public sealed class PayersRepository : IPayersRepository
{
    private static readonly Dictionary<Guid, Payer> _database = new();


    public IEnumerable<Payer> List()
        => _database.Values;

    public Payer? Get(Guid id)
    {
        _database.TryGetValue(id, out var payer);
        return payer;
    }


    public bool Any(Guid id)
        => _database.ContainsKey(id);

    public void Add(Payer payer)
        => _database.Add(payer.Id, payer);

    public void Update(Payer payer)
        => _database[payer.Id] = payer;

    public void Delete(Guid id)
        => _database.Remove(id);
}
