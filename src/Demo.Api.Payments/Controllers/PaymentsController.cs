using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Application.Payments.GetPayment;
using Demo.Api.Payments.Application.Payments.GetPayments;
using Demo.Api.Payments.Application.Payments.Pay;
using Demo.BuildingBlocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.Api.Payments.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;
    private readonly IMediator _dispatcher;

    public PaymentsController(
        ILogger<PaymentsController> logger,
        IMediator dispatcher
    )
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }


    [HttpGet]
    public async Task<IActionResult> GetPayments(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("[API][PAYMENTS][GET] All");

            var response = await _dispatcher.Send(GetPaymentsQuery.Instance, cancellationToken);

            return Ok(response);
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{id:guid}", Name = nameof(GetPayment))]
    public async Task<IActionResult> GetPayment(Guid id, CancellationToken cancellationToken)
    {
        try
        {

            _logger.LogInformation($"[API][PAYMENTS][GET] Id: {id}");
            var response = await _dispatcher.Send(new GetPaymentQuery(id), cancellationToken);

            return Ok(response);
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Pay(PayCommand command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"[API][PAYMENTS][PAY] Payload: {command}");

            var response = await _dispatcher.Send(command, cancellationToken);

            return new CreatedAtRouteResult(
                nameof(GetPayment),
                new { id = response.PaymentId },
                response);
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
