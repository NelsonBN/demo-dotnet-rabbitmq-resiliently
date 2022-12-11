using System.Collections.Generic;
using MediatR;

namespace Demo.Api.Customers.Application.GetCustomers;

public sealed record GetCustomersQuery : IRequest<IEnumerable<CustomerSummaryResponse>>
{
    public static readonly GetCustomersQuery Instance = new();
}
