using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Techshop.Models.Entities;

namespace Techshop.Repository.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(e => new { e.UserId, e.ProductId });

        builder.HasOne(e => e.User).WithMany(e => e.CartItems).HasForeignKey(e => e.UserId);
    }
}