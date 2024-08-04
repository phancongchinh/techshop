using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techshop.Models.Entities;

[Table("image")]
public class Image
{
    [Key] public int Id { get; set; }

    public string Path { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; }
}