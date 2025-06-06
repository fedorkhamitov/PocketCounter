using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketCounter.Application.Dtos;

namespace PocketCounter.Infrastructure.Configurations.Read;

public class OrderDtoConfiguration : IEntityTypeConfiguration<OrderDto>
{
    public void Configure(EntityTypeBuilder<OrderDto> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Address)
            .HasColumnName("address")
            .HasConversion(
                a => JsonSerializer.Serialize(a, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }),
                s => JsonSerializer.Deserialize<AddressDto>(s, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!
            );

        builder.Property(o => o.CartLines)
            .HasColumnName("cart_lines")
            //.HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }),
                v => JsonSerializer.Deserialize<CartLineDto[]>(v, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!
            );
    }
}