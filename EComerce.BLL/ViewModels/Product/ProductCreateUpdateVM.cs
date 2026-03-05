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

        // Image (only for upload; not mapped to DB directly)
        [Display(Name = "Product Image")]
        public IFormFile? ImageFile { get; set; }

        /// <summary>Relative path stored in DB, e.g. /images/products/abc.jpg</summary>
        public string? ImageUrl { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
            = new List<SelectListItem>();
    }
}
