using System.ComponentModel.DataAnnotations.Schema;
using Techshop.Models.enums;

namespace Techshop.Models.Entities;

[Table("app_order")]
public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime CompletedAt { get; set; }

    public virtual User User { get; set; }

    public virtual IEnumerable<OrderItem> OrderItems { get; set; }
}