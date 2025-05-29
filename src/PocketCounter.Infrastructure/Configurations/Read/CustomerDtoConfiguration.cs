using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketCounter.Application.Dtos;

namespace PocketCounter.Infrastructure.Configurations.Read;

public class CustomerDtoConfiguration : IEntityTypeConfiguration<CustomerDto>
{
    public void Configure(EntityTypeBuilder<CustomerDto> builder)
    {
        builder.ToTable("customers");
        
        builder.HasKey(c => c.Id);

        builder.ComplexProperty(c => c.Name, b =>
        {
            b.Property(n => n.FirstName)
                .HasColumnName("first_name");
            b.Property(n => n.Patronymic)
                .HasColumnName("patronymic");
            b.Property(n => n.FamilyName)
                .HasColumnName("family_name");
        });
        
        builder.Property(c => c.PhoneNumber)
            .HasColumnName("phone_number")
            .HasConversion(
                v => v,
                v => ExtractInternationalFormat(v)
            );
        
        builder.HasMany(c => c.Orders)
            .WithOne()
            .HasForeignKey(o => o.CustomerId);
    }
    
    private static string ExtractInternationalFormat(string json)
    {
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("internationalFormat").GetString()!;
    }
}