using System.Collections.Generic;
using MediatR;

namespace Demo.Api.Payments.Application.Payments.GetPayments;

public sealed record GetPaymentsQuery : IRequest<IEnumerable<PaymentSummaryResponse>>
{
    public static readonly GetPaymentsQuery Instance = new();
}
