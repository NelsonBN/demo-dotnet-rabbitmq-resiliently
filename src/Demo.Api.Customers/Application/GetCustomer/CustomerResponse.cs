using System;

namespace Demo.Api.Customers.Application.GetCustomer;

public sealed record CustomerResponse(
    Guid CustomerId,
    string Name,
    string Address,
    string PhotoUrl
);
