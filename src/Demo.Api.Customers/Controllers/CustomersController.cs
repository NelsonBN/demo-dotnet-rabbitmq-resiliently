using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Api.Customers.Application.CreateCustomer;
using Demo.Api.Customers.Application.DeleteCustomer;
using Demo.Api.Customers.Application.GetCustomer;
using Demo.Api.Customers.Application.GetCustomers;
using Demo.Api.Customers.Application.UpdateCustomer;
using Demo.Api.Customers.Application.UpdateCustomerPhoto;
using Demo.BuildingBlocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.Api.Customers.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private readonly IMediator _dispatcher;

    public CustomersController(
        IMediator dispatcher,
        ILogger<CustomersController> logger
    )
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("[API][CUSTOMERS][GET] All");

            var response = await _dispatcher.Send(GetCustomersQuery.Instance, cancellationToken);

            return Ok(response);
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{id:guid}", Name = nameof(GetCustomer))]
    public async Task<IActionResult> GetCustomer(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"[API][CUSTOMERS][GET] Id: {id}");
            var response = await _dispatcher.Send(new GetCustomerQuery(id), cancellationToken);

            return Ok(response);
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"[API][CUSTOMERS][CREATE] Payload: {command}");

            var response = await _dispatcher.Send(command, cancellationToken);

            return new CreatedAtRouteResult(
                nameof(GetCustomer),
                new { id = response.CustomerId },
                response);
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCustomer(
        [FromRoute] Guid id,
        [FromBody] UpdateCustomerCommand command,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            command.Id = id;

            _logger.LogInformation($"[API][CUSTOMERS][UPDATE] Payload: {command}");

            await _dispatcher.Send(command, cancellationToken);
            return NoContent();
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}/photo")]
    public async Task<IActionResult> UpdateCustomerPhoto(
        [FromRoute] Guid id,
        UpdateCustomerPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            command.Id = id;

            _logger.LogInformation($"[API][CUSTOMERS][UPDATE][PHOTO] Payload: {command}");

            await _dispatcher.Send(command, cancellationToken);

            return NoContent();
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCustomer(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"[API][CUSTOMERS][DELETE] Id: {id}");

            await _dispatcher.Send(new DeleteCustomerCommand(id), cancellationToken);

            return NoContent();
        }
        catch(DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
