using System;

namespace Demo.Api.Payments.Application.Payers.GetPayers;

public sealed record PayerSummaryResponse(
    Guid PayerId,
    string Name);
