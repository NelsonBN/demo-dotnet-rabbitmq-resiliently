using System;
using MediatR;

namespace Demo.Api.Payments.Application.Payers.GetPayer;

public sealed record GetPayerQuery(Guid id) : IRequest<PayerResponse>;
