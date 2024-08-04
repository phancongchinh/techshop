using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techshop.Models.Entities;

[Table("user_role")]
public class Role
{
    [Key] public int Id { get; set; }

    public string Name { get; set; }

    public virtual IEnumerable<User> Users { get; set; }
}