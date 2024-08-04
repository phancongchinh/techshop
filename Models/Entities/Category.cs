using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techshop.Models.Entities;

[Table("category")]
public class Category
{
    [Key] public int Id { get; set; }

    public int Name { get; set; }

    public virtual IEnumerable<Product> Products { get; set; }
}