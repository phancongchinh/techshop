using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Techshop.Models.Entities;

namespace Techshop.Models.ViewModels;

public class ProductVm
{
    [Required]
    [MaxLength(64)]
    public string Brand { get; set; }

    [Required]
    [MaxLength(64)]
    public string Model { get; set; }

    [Required]
    [Min(0)]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(255)]
    public string Description { get; set; }

    [Required]
    [Min(0)]
    public int Quantity { get; set; }

    [Required]
    public int[] SelectedCategoryIds { get; set; }


    public IEnumerable<Category>? Categories { get; set; }

    public List<IFormFile>? Images { get; set; }
}