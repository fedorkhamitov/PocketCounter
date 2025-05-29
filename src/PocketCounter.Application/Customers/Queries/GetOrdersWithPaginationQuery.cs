namespace PocketCounter.Application.Customers.Queries;

public record GetOrdersWithPaginationQuery(int Page, int PageSize);