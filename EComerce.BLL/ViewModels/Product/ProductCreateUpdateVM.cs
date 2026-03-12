using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.ViewModels.Product
{
    public class ProductCreateUpdateVM
    {
        public int? Id { get; set; }

        public string Name { get; set; } = null!;

        public string SKU { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }


        [Display(Name = "Product Image")]
        public IFormFile? ImageFile { get; set; }


        public string? ImageUrl { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
            = new List<SelectListItem>();
    }
}
