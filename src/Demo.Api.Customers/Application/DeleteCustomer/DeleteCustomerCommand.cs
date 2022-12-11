using System;
using MediatR;

namespace Demo.Api.Customers.Application.DeleteCustomer;

public sealed record DeleteCustomerCommand(Guid Id) : IRequest;
