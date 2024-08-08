using Techshop.Models.Entities;
using Techshop.Models.enums;

namespace Techshop.Models.ViewModels;

public class OrderUpdateVm
{
    public Order? Order { get; set; }

    public OrderStatus Status { get; set; }

    public List<OrderStatus>? OrderStatusValues { get; set; }
}