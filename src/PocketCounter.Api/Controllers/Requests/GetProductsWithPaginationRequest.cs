using PocketCounter.Application.Categories.Queries.GetCategoriesWithPagination;

namespace PocketCounter.Api.Controllers.Requests;

public record GetProductsWithPaginationRequest(int Page, int PageSize)
{
    public GetProductsWithPaginationQuery ToQuery() =>
        new(Page, PageSize);
}