using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PocketCounter.Application.Categories;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;
using PocketCounter.Infrastructure.DbContexts.Write;

namespace PocketCounter.Infrastructure.Repositories;

public class CategoryRepository(WriteDbContext context) : ICategoryRepository
{
    public async Task<Result<Guid, Error>> Add(Category category, CancellationToken cancellationToken)
    {
        var isNameAlreadyExists = await context.Categories
            .AnyAsync(c => EF.Functions.ILike(c.Name, category.Name), cancellationToken);
        if (isNameAlreadyExists)
            return Errors.General.ValueIsAlreadyExists("Category Name");

        await context.Categories.AddAsync(category, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Errors.General.ValueIsAlreadyExists("Category Name");
        }
    }

    public async Task<Result<Category, Error>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category == null)
            return Errors.General.NotFound(id);

        return category;
    }

    public async Task<Guid> Save(Category category, CancellationToken cancellationToken)
    {
        context.Categories.Attach(category);

        await context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }

    public async Task<Result<bool, Error>> IsProductAlreadyExistsByTitle(
        Title title, CancellationToken cancellationToken)
    {
        var exist = await context.Categories
            .SelectMany(c => c.Products)
            .AnyAsync(p => p.Title == title, cancellationToken);

        return exist;
    }

    public async Task<Result<bool, Error>> IsProductAlreadyExistsBySku(
        Sku sku, CancellationToken cancellationToken)
    {
        var exist = await context.Categories
            .SelectMany(c => c.Products)
            .AnyAsync(p => p.Sku == sku, cancellationToken);

        return exist;
    }

    public async Task<Guid> Remove(Category category, CancellationToken cancellationToken)
    {
        context.Categories.Remove(category);

        await context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }

    public async Task<Result<Product, Error>> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Products.Any(p => p.Id == id), cancellationToken);

        return category == null ? 
            Errors.General.NotFound(id)
            : category.Products.Single(p => p.Id == id);
    }
}