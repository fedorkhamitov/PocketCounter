using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketCounter.Api.Extensions;
using PocketCounter.Api.Response;
using PocketCounter.Application.Categories.Products.Update;
using PocketCounter.Application.Customers.Create;
using PocketCounter.Application.Customers.Delete;
using PocketCounter.Application.Customers.Orders.Create;
using PocketCounter.Application.Customers.Update;
using PocketCounter.Application.Customers.Orders.Delete;
using PocketCounter.Application.Customers.Orders.Update;
using PocketCounter.Application.Customers.Queries.GetCustomersList;
using PocketCounter.Application.Dtos;

namespace PocketCounter.Api.Controllers;

[ApiController]
[Route("customer")]
[Authorize]
public class CustomerController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] IValidator<CreateCustomerRequest> validator,
        [FromServices] CreateCustomerHandler handler,
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpDelete("{id:guid}/")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteCustomerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, false, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult<Guid>> DeleteHard(
        [FromRoute] Guid id,
        [FromServices] DeleteCustomerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, true, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{id:guid}/settings")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromServices] UpdateCustomerHandler handler,
        [FromServices] IValidator<UpdateCustomerRequest> validator,
        [FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());

        var result = await handler.Handle(id, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpPost("{id:guid}/order")]
    public async Task<ActionResult<Guid>> CreateOrder(
        [FromRoute] Guid id,
        [FromServices] IValidator<CreateOrderRequest> validator,
        [FromServices] CreateOrderHandler handler,
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());

        var result = await handler.Handle(id, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpDelete("{id:guid}/order/{orderId:guid}/hard")]
    public async Task<ActionResult<Guid>> DeleteOrderHard(
        [FromRoute] Guid id,
        [FromRoute] Guid orderId,
        [FromServices] DeleteOrderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, orderId, true, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpDelete("{id:guid}/order/{orderId:guid}")]
    public async Task<ActionResult<Guid>> DeleteOrder(
        [FromRoute] Guid id,
        [FromRoute] Guid orderId,
        [FromServices] DeleteOrderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, orderId, false, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpPut("{id:guid}/order/{orderId:guid}/cartlines")]
    public async Task<ActionResult<Guid>> UpdateOrderCartLines(
        [FromRoute] Guid id,
        [FromRoute] Guid orderId,
        [FromServices] UpdateOrderCartLinesHandler handler,
        [FromBody] UpdateOrderCartLinesRequest request,
        [FromServices] IValidator<UpdateOrderCartLinesRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());

        var result = await handler.Handle(id, orderId, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{id:guid}/order/{orderId:guid}/status")]
    public async Task<ActionResult<Guid>> UpdateOrderStatus(
        [FromRoute] Guid id,
        [FromRoute] Guid orderId,
        [FromServices] UpdateOrderStatusHandler handler,
        [FromBody] UpdateOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, orderId, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<CustomerDto>>> GetAllCategories(
        [FromServices] GetCustomersListHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{id:guid}/order/{orderId:guid}/address")]
    public async Task<ActionResult<Guid>> UpdateOrderAddress(
        [FromRoute] Guid id,
        [FromRoute] Guid orderId,
        [FromServices] UpdateOrderAddressHandler handler,
        [FromBody] UpdateOrderAddressRequest request,
        [FromServices] IValidator<UpdateOrderAddressRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());

        var result = await handler.Handle(id, orderId, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
}