using Techshop.Models.Entities;

namespace Techshop.Models.ViewModels;

public class HomeVm
{
    public string Search { get; set; }

    public int CategoryId { get; set; }

    public IEnumerable<Category> Categories { get; set; }

    public IEnumerable<Product>? Products { get; set; }
}