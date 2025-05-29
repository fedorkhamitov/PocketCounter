using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Infrastructure.Configurations.Write;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Sku)
            .HasConversion(sku => sku.Value, value => Sku.Create(value).Value)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .HasColumnName("sku");
        ;

        builder.Property(p => p.Title)
            .HasConversion(title => title.Value, value => Title.Create(value).Value)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .HasColumnName("title");

        builder.Property(p => p.Description)
            .HasConversion(d => d.Value, value => Description.Create(value).Value)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGHT)
            .HasColumnName("description");

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.CostPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.OwnsOne(p => p.PhotosList, b =>
        {
            b.ToJson();
            b.OwnsMany(ph => ph.Photos, phb =>
            {
                phb.Property(pl => pl.PathToStorage)
                    .HasConversion(fp => fp.Path, value => FilePath.Create(value).Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
            });
        });

        builder.OwnsOne(p => p.Dimensions, b =>
        {
            b.ToJson();
            b.Property(d => d.Height)
                .IsRequired();
            b.Property(d => d.Width)
                .IsRequired();
            b.Property(d => d.Depth)
                .IsRequired();
        });

        builder.Property(p => p.Weigth)
            .IsRequired()
            .HasColumnType("float");

        builder.OwnsOne(p => p.ProductQuantity, b =>
        {
            b.Property(pq => pq.ActualQuantity)
                .IsRequired()
                .HasColumnName("actual_quantity");
            b.Property(pq => pq.QuantityForShipping)
                .IsRequired()
                .HasColumnName("quantity_for_shipping");
            b.Property(pq => pq.ReservedQuantity)
                .IsRequired()
                .HasColumnName("reserved_quantity");
        });
        
        builder.Property<bool>("IsDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}