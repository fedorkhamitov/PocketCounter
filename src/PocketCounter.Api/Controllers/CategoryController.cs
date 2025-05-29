using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketCounter.Api.Extensions;
using PocketCounter.Api.Response;
using PocketCounter.Application.Categories.Create;
using PocketCounter.Application.Categories.Delete;
using PocketCounter.Application.Categories.Products.Create;
using PocketCounter.Application.Categories.Products.Delete;
using PocketCounter.Application.Categories.Products.Update;
using PocketCounter.Application.Categories.Update;

namespace PocketCounter.Api.Controllers;

[ApiController]
[Route("category")]
[Authorize]
public class CategoryController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] IValidator<CreateCategoryRequest> validator,
        [FromServices] CreateCategoryHandler handler,
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpPost("{id:guid}/product")]
    public async Task<ActionResult<Guid>> CreatProduct(
        [FromRoute] Guid id,
        [FromServices] IValidator<CreateProductRequest> validator,
        [FromServices] CreateProductHandler handler,
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());

        var result = await handler.Handle(request, id, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpDelete("{id:guid}/")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteCategoryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, false, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult<Guid>> DeleteHard(
        [FromRoute] Guid id,
        [FromServices] DeleteCategoryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, true, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpDelete("{id:guid}/product/{productId:guid}/hard")]
    public async Task<ActionResult<Guid>> DeleteProductHard(
        [FromRoute] Guid id,
        [FromRoute] Guid productId,
        [FromServices] DeleteProductHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, productId, true, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpDelete("{id:guid}/product/{productId:guid}")]
    public async Task<ActionResult<Guid>> DeleteProduct(
        [FromRoute] Guid id,
        [FromRoute] Guid productId,
        [FromServices] DeleteProductHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, productId, false, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpPut("{id:guid}/settings")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromServices] UpdateCategoryHandler handler,
        [FromServices] IValidator<UpdateCategoryRequest> validator,
        [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());

        var result = await handler.Handle(id, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }

    [HttpPut("{id:guid}/product/{productId:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateProductMainInfo(
        [FromRoute] Guid id,
        [FromRoute] Guid productId,
        [FromServices] UpdateProductMainInfoHandler handler,
        [FromServices] IValidator<UpdateProductMainInfoRequest> validator,
        [FromBody] UpdateProductMainInfoRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        
        var result = await handler.Handle(id, productId, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{id:guid}/product/{productId:guid}/prices")]
    public async Task<ActionResult<Guid>> UpdateProductPrices(
        [FromRoute] Guid id,
        [FromRoute] Guid productId,
        [FromServices] UpdateProductPriceHandler handler,
        [FromServices] IValidator<UpdateProductPriceRequest> validator,
        [FromBody] UpdateProductPriceRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        
        var result = await handler.Handle(id, productId, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{id:guid}/product/{productId:guid}/dimensions")]
    public async Task<ActionResult<Guid>> UpdateProductDimensions(
        [FromRoute] Guid id,
        [FromRoute] Guid productId,
        [FromServices] UpdateProductDimensionsHandler handler,
        [FromServices] IValidator<UpdateProductDimensionsRequest> validator,
        [FromBody] UpdateProductDimensionsRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        
        var result = await handler.Handle(id, productId, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{id:guid}/product/{productId:guid}/quantity")]
    public async Task<ActionResult<Guid>> UpdateProductDimensions(
        [FromRoute] Guid id,
        [FromRoute] Guid productId,
        [FromServices] UpdateProductQuantityHandler handler,
        [FromServices] IValidator<UpdateProductQuantityRequest> validator,
        [FromBody] UpdateProductQuantityRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ValidationResultErrorEnvelope());
        
        var result = await handler.Handle(id, productId, request, cancellationToken);

        return result.IsFailure ? result.Error.ToResponse() : Ok(Envelope.Ok(result.Value));
    }
}