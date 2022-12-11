using System;

namespace Demo.Api.Payments.Application.Payers.GetPayer;

public sealed record PayerResponse(
    Guid PayerId,
    string Name,
    string Address);
