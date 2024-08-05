using System.ComponentModel.DataAnnotations.Schema;

namespace Techshop.Models.Entities;

[Table("product")]
public class Product
{
    public int Id { get; set; }

    public string Brand { get; set; }

    public string Model { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public virtual ICollection<Image> Images { get; set; }

    public virtual IEnumerable<Category> Categories { get; set; }

    public virtual string Name => Brand + " - " + Model;
}