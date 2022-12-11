using MediatR;

namespace Demo.Api.Customers.Application.CreateCustomer;

public sealed record CreateCustomerCommand(string Name, string Address) : IRequest<CreateCustomerResponse>;
