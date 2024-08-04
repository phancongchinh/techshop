using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Techshop.Models.Entities;

namespace Techshop.Repository.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(e => new { e.OrderId, e.ProductId });
        builder.Property(e => e.Price).HasColumnType("decimal(10,2)");

        builder.HasOne(e => e.Order).WithMany(e => e.OrderItems)
            .HasForeignKey(e => e.OrderId).OnDelete(DeleteBehavior.Cascade);
    }
}