using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketCounter.Application.Dtos;

namespace PocketCounter.Infrastructure.Configurations.Read;

public class ProductDtoConfiguration : IEntityTypeConfiguration<ProductDto>
{
    public void Configure(EntityTypeBuilder<ProductDto> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Dimensions)
            .HasColumnName("dimensions")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<DimensionsDto>(v, (JsonSerializerOptions?)null)!
            );
    }
}