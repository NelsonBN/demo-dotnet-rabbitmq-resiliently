using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Demo.Api.Customers.Application.UpdateCustomerPhoto;

public sealed record UpdateCustomerPhotoCommand : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string PhotoUrl { get; set; } = default!;
}
