using System;

namespace Demo.Api.Customers.Application.GetCustomers;

public sealed record CustomerSummaryResponse(
    Guid CustomerId,
    string Name);
