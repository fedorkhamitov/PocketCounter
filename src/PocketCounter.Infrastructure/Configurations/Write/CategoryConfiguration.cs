using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;

namespace PocketCounter.Infrastructure.Configurations.Write;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGHT);

        builder.HasMany(c => c.Products)
            .WithOne()
            .HasForeignKey("category_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property<bool>("IsDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}