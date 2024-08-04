using System.ComponentModel.DataAnnotations.Schema;

namespace Techshop.Models.Entities;

[Table("cart_item")]
public class CartItem
{
    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual User User { get; set; }
    
    public virtual Product Product { get; set; }
}