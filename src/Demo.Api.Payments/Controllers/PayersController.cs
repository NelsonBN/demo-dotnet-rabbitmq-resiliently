using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Payments.Application.Payers.GetPayer;
using Demo.Api.Payments.Application.Payers.GetPayers;
using Demo.BuildingBlocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.Api.Payments.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class PayersController : ControllerBase
{
    private readonly ILogger<PayersController> _logger;
    private readonly IMediator _dispatcher;

    public PayersController(
        ILogger<PayersController> logger,
        IMediator dispatcher
    )
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> GetPayers(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("[API][PAYERS][GET] All");

            var response = await _dispatcher.Send(GetPayersQuery.Instance, cancellationToken);

            return Ok(response);
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{id:guid}", Name = nameof(GetPayers))]
    public async Task<IActionResult> GetPayers(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"[API][PAYERS][GET] Id: {id}");
            var response = await _dispatcher.Send(new GetPayerQuery(id), cancellationToken);

            return Ok(response);
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
