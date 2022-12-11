using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Demo.Api.Customers.Application.UpdateCustomer;

public sealed record UpdateCustomerCommand : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
}
