using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techshop.Models.Entities;

[Table("app_user")]
public class User
{
    [Key] public int Id { get; set; }

    public string Username { get; set; }

    public string FullName { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public int RoleId { get; set; }


    public virtual Role Role { get; set; }

    public virtual IEnumerable<Order> Orders { get; set; }

    public virtual IEnumerable<CartItem> CartItems { get; set; }
}