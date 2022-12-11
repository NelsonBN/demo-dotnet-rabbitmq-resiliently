using System.Collections.Generic;
using MediatR;

namespace Demo.Api.Payments.Application.Payers.GetPayers;

public sealed record GetPayersQuery : IRequest<IEnumerable<PayerSummaryResponse>>
{
    public static readonly GetPayersQuery Instance = new();
}
