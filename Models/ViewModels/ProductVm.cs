using Microsoft.AspNetCore.Mvc.Rendering;
using Techshop.Models.Entities;

namespace Techshop.Models.ViewModels;

public class ProductVm
{
    public string Brand { get; set; }

    public string Model { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public int[] SelectedCategoryIds { get; set; }


    public IEnumerable<Category>? Categories { get; set; }

    public List<IFormFile> Images { get; set; }
}