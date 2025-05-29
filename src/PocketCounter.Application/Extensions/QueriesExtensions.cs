using Microsoft.EntityFrameworkCore;
using PocketCounter.Application.Dtos;
using PocketCounter.Application.Models;

namespace PocketCounter.Application.Extensions;

public static class QueriesExtensions
{
    public static async Task<PagedList<T>> ToPagedList<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await source.CountAsync(cancellationToken);

        var item = await source.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<T>()
        {
            Items = item,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}