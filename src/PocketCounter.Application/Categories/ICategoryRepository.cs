using CSharpFunctionalExtensions;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Categories;

public interface ICategoryRepository
{
    Task<Result<Guid, Error>> Add(Category category, CancellationToken cancellationToken);
    
    Task<Result<Category, Error>> GetById(Guid id, CancellationToken cancellationToken);
    
    Task<Guid> Save(Category category, CancellationToken cancellationToken);
    
    Task<Result<bool, Error>> IsProductAlreadyExistsByTitle(Title title, CancellationToken cancellationToken);

    Task<Result<bool, Error>> IsProductAlreadyExistsBySku(Sku sku, CancellationToken cancellationToken);

    Task<Guid> Remove(Category category, CancellationToken cancellationToken);

    Task<Result<Product, Error>> GetProductById(Guid id, CancellationToken cancellationToken);
}