using PocketCounter.Application.Customers.Queries;

namespace PocketCounter.Api.Controllers.Requests;

public record GetOrdersWithPaginationRequest(int Page, int PageSize)
{
    public GetOrdersWithPaginationQuery ToQuery() =>
        new(Page, PageSize);
}