using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Techshop.Models.Entities;

namespace Techshop.Repository.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Price).HasColumnType("decimal(18,2)");

        // builder.HasMany(e => e.Images).WithOne(e => e.Product).HasForeignKey(e => e.ProductId);
        builder.HasMany(e => e.Categories).WithMany(e => e.Products);
    }
}