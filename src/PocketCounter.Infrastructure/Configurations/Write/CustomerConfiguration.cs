using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;

namespace PocketCounter.Infrastructure.Configurations.Write;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        
        builder.HasKey(c => c.Id);
        
        builder.ComplexProperty(c => c.Name, b =>
        {
            b.Property(n => n.FirstName)
                .IsRequired()
                .HasColumnName("first_name")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
            b.Property(n => n.Patronymic)
                .IsRequired()
                .HasColumnName("patronymic")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
            b.Property(n => n.FamilyName)
                .IsRequired()
                .HasColumnName("family_name")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
        });
        
        builder.OwnsOne(c => c.PhoneNumber, pnBuilder =>
        {
            pnBuilder.ToJson();
            pnBuilder.Property(phoneNumber => phoneNumber.Number)
                .HasJsonPropertyName("number");
            pnBuilder.Property(phoneNumber => phoneNumber.CountryCode)
                .HasJsonPropertyName("countryCode");
            pnBuilder.Property(phoneNumber => phoneNumber.NationalNumber)
                .HasJsonPropertyName("nationalNumber");
            pnBuilder.Property(phoneNumber => phoneNumber.InternationalFormat)
                .HasJsonPropertyName("internationalFormat");
            pnBuilder.Property(phoneNumber => phoneNumber.Type)
                .HasConversion<string>()
                .HasJsonPropertyName("type");
        });
        
        builder.HasMany(c => c.Orders)
            .WithOne()
            .HasForeignKey("customer_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}