using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketCounter.Application.Dtos;

namespace PocketCounter.Infrastructure.Configurations.Read;

public class CategoryDtoConfiguration : IEntityTypeConfiguration<CategoryDto>
{
    public void Configure(EntityTypeBuilder<CategoryDto> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.Products)
            .WithOne()
            .HasForeignKey(p => p.CategoryId);
    }
}