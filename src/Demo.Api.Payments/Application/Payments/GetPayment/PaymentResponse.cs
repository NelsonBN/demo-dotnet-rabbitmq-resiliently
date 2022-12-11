using System;

namespace Demo.Api.Payments.Application.Payments.GetPayment;

public sealed record PaymentResponse(
    Guid PaymentId,
    Guid PayerId,
    string PayerName,
    double Amount,
    DateTime CreatedAt);
