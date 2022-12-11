using System;
using System.Collections.Generic;

namespace Demo.Api.Payments.Domain.Payments;

public interface IPaymentsRepository
{
    IEnumerable<Payment> List();
    Payment? Get(Guid id);
    void Add(Payment payment);
}
