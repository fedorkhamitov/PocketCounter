using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PocketCounter.Application.Database;
using PocketCounter.Application.Dtos;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Categories.Queries.GetCategoriesList;

public class GetCategoriesListHandler(IReadDbContext readDbContext)
{
    private readonly IReadDbContext _readDbContext = readDbContext;

    public async Task<Result<List<CategoryDto>, Error>> Handle(CancellationToken cancellationToken)
    {
        return await _readDbContext.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Description = c.Description,
                Name = c.Name
            }).
            ToListAsync(cancellationToken);
    }
}