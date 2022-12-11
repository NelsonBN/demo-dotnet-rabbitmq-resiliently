using System;
using System.Collections.Generic;

namespace Demo.Api.Customers.Domain;

public interface ICustomersRepository
{
    IEnumerable<Customer> List();
    Customer? Get(Guid id);

    bool Any(string name, Guid? id = null);
    bool Any(Guid id);

    void Add(Customer customer);
    void Update(Customer customer);

    void Delete(Customer customer);
}
