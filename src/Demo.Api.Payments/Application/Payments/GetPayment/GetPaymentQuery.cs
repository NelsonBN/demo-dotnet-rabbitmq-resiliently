using System;
using MediatR;

namespace Demo.Api.Payments.Application.Payments.GetPayment;

public sealed record GetPaymentQuery(Guid id) : IRequest<PaymentResponse>;
