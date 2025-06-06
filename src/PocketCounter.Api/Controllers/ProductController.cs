using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketCounter.Api.Controllers.Requests;
using PocketCounter.Api.Response;
using PocketCounter.Application.Categories.Queries.GetCategoriesWithPagination;
using PocketCounter.Application.Dtos;
using PocketCounter.Application.Models;

namespace PocketCounter.Api.Controllers;

[ApiController]
[Route("product")]
public class ProductController
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Envelope<PagedList<ProductDto>>>> Get(
        [FromQuery] GetProductsWithPaginationRequest request,
        [FromServices] GetCategoriesWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var response = await handler.Handle(query, cancellationToken);

        return Envelope<PagedList<ProductDto>>.Ok(response);
    }
}