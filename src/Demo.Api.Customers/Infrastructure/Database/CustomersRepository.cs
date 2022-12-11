using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Api.Customers.Domain;

namespace Demo.Api.Customers.Infrastructure.Database;

public sealed class CustomersRepository : ICustomersRepository
{
    private static readonly Dictionary<Guid, Customer> _database = new();


    public IEnumerable<Customer> List()
        => _database.Values;

    public Customer? Get(Guid id)
    {
        _database.TryGetValue(id, out var customer);
        return customer;
    }

    public bool Any(string name, Guid? id = null)
        => _database.Any(a =>
            (id is null || a.Key != id) &&
            a.Value.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
        );

    public bool Any(Guid id)
        => _database.ContainsKey(id);

    public void Add(Customer customer)
        => _database.Add(customer.Id, customer);

    public void Update(Customer customer)
        => _database[customer.Id] = customer;

    public void Delete(Customer customer)
        => _database.Remove(customer.Id);
}
