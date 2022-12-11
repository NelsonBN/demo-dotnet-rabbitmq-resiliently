using System;
using MediatR;

namespace Demo.Api.Payments.Application.Payments.Pay;

public sealed record PayCommand(Guid PayerId, double Amount) : IRequest<PayResponse>;
