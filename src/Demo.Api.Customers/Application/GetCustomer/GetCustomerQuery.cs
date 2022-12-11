using System;
using MediatR;

namespace Demo.Api.Customers.Application.GetCustomer;

public sealed record GetCustomerQuery(Guid Id) : IRequest<CustomerResponse>;
