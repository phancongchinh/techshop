using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Techshop.Models.Entities;

namespace Techshop.Repository.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.HasOne(e => e.User).WithMany(e => e.Orders).HasForeignKey(e => e.UserId);
        // builder.HasMany(e => e.OrderItems).WithOne(e => e.Order).HasForeignKey(e => e.OrderId);
    }
}