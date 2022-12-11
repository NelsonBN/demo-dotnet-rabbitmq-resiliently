using System;

namespace Demo.Api.Payments.Application.Payments.GetPayments;

public sealed record PaymentSummaryResponse(
    Guid PaymentId,
    string PayerName,
    double Amount);
