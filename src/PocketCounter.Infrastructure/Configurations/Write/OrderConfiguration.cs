using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Infrastructure.Configurations.Write;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.CartLines)
            .HasColumnType("jsonb")
            .HasConversion(list =>
                    JsonSerializer.Serialize(list, JsonSerializerOptions.Default),
                value => JsonSerializer.Deserialize<IReadOnlyList<CartLine>>(value,
                    JsonSerializerOptions.Default)!,
                new ValueComparer<IReadOnlyList<CartLine>>(
                    (l1, l2) => l1!.SequenceEqual(l2!),
                    l => l.Aggregate(0, (current, value) => HashCode.Combine(current, value.GetHashCode())),
                    l => l.ToList()));

        builder.Property(o => o.SerialNumber)
            .HasConversion(sn => sn.Value, value => SerialNumber.Create(value).Value);
        
        builder.OwnsOne(o => o.Address, b =>
        {
            b.ToJson();
            b.Property(a => a.ZipCode).IsRequired();
            b.Property(a => a.Country).IsRequired();
            b.Property(a => a.State).IsRequired();
            b.Property(a => a.City).IsRequired();
            b.Property(a => a.StreetName).IsRequired();
            b.Property(a => a.StreetNumber).IsRequired();
            b.Property(a => a.Apartment).IsRequired();
            b.Property(a => a.SpecialAddressString).IsRequired();
        });

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(o => o.IsPaid)
            .IsRequired();

        builder.Property(o => o.CreateDateTime)
            .IsRequired();

        builder.Property(o => o.Comment)
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGHT);

        builder.Property(o => o.TotalPrice);
    }
}