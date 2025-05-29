using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.Entities;

public class Category : SoftDeletableEntity
{
    private Category() { }
    
    public Category(string name, string description = "")
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }
    
    public new Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    
    public IReadOnlyList<Product> Products => _products;
    
    private readonly List<Product> _products = [];
    
    public UnitResult<Error> AddProduct(Product product)
    {
        _products.Add(product);
        return Result.Success<Error>();
    }

    public UnitResult<Error> RemoveProduct(Product product)
    {
        _products.Remove(product);
        return Result.Success<Error>();
    }
    
    public override void Delete()
    {
        base.Delete();
        if (Products.Count == 0) return;
        foreach (var product in Products)
        {
            product.Delete();
        }
    }
    
    public override void Restore()
    {
        base.Restore();
        if (Products.Count == 0) return;
        foreach (var product in Products)
        {
            product.Restore();
        }
    }

    public UnitResult<Error> UpdateMainInfo(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired("Category name");
        
        Name = name;
        Description = description;
        
        return Result.Success<Error>();
    }
}