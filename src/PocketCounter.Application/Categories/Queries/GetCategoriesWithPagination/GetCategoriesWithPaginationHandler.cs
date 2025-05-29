using PocketCounter.Application.Database;
using PocketCounter.Application.Dtos;
using PocketCounter.Application.Extensions;
using PocketCounter.Application.Models;

namespace PocketCounter.Application.Categories.Queries.GetCategoriesWithPagination;

public class GetCategoriesWithPaginationHandler(IReadDbContext readDbContext)
{
    private readonly IReadDbContext _readDbContext = readDbContext;

    public async Task<PagedList<ProductDto>> Handle(
        GetProductsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var productsQuery = _readDbContext.Products;

        // Будущая фильтрация и сортировка
        
        return await productsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}