using System.ComponentModel.DataAnnotations.Schema;

namespace Techshop.Models.Entities;

[Table("order_item")]
public class OrderItem
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }


    public virtual Order Order { get; set; }

    public virtual Product Product { get; set; }
}