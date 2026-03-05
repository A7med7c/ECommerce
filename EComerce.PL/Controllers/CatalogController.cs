using ECommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.PL.Controllers
{
    /// <summary>
    /// Public product catalog.
    /// No [Authorize] — browsing is open to all visitors.
    /// </summary>
    public class CatalogController(
        IProductService _productService,
        IFavoriteService _favoriteService) : Controller
    {
        // ── GET /Catalog ──────────────────────────────────────────────────
        public async Task<IActionResult> Index(
            int? categoryId,
            string? q,
            string sort = "newest",
            int page = 1)
        {
            var vm = await _productService.GetCatalogAsync(categoryId, q, sort, page);
            return View(vm);
        }

        // ── GET /Catalog/Details/5 ────────────────────────────────────────
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var product = await _productService.GetProductAsync(id.Value);
            if (product is null) return NotFound();

            // Pass whether the current user has already favourited this product
            bool isFav = false;
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
                isFav = await _favoriteService.IsFavoriteAsync(userId, product.Id);
            }

            ViewBag.IsFavorite = isFav;
            return View(product);
        }
    }
}
