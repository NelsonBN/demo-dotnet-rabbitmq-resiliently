using System;
using System.Collections.Generic;

namespace Demo.Api.Payments.Domain.Payers;

public interface IPayersRepository
{
    IEnumerable<Payer> List();
    Payer? Get(Guid id);
    bool Any(Guid id);
    void Add(Payer payer);
    void Update(Payer payer);
    void Delete(Guid id);
}
