using PocketCounter.Application.Database;
using PocketCounter.Application.Dtos;
using PocketCounter.Application.Extensions;
using PocketCounter.Application.Models;

namespace PocketCounter.Application.Customers.Queries;

public class GetCustomersWithPaginationHandler(IReadDbContext readDbContext)
{
    private readonly IReadDbContext _readDbContext = readDbContext;
    public async Task<PagedList<OrderDto>> Handle(
        GetOrdersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var ordersQuery = _readDbContext.Orders;

        // Будущая фильтрация и сортировка
        
        return await ordersQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}