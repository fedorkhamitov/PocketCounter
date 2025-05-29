using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketCounter.Api.Controllers.Requests;
using PocketCounter.Api.Response;
using PocketCounter.Application.Customers.Queries;
using PocketCounter.Application.Dtos;
using PocketCounter.Application.Models;

namespace PocketCounter.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Envelope<PagedList<OrderDto>>>> Get(
        [FromQuery] GetOrdersWithPaginationRequest request,
        [FromServices] GetCustomersWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var response = await handler.Handle(query, cancellationToken);

        return Envelope<PagedList<OrderDto>>.Ok(response);
    }
}