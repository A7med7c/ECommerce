using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class ProductCreateUpdateVM
{
    public int? Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string SKU { get; set; } = null!;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public bool IsActive { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; }
        = new List<SelectListItem>();
}