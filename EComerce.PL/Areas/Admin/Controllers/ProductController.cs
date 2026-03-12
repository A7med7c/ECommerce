using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController(
        IProductService _productService,
        ICategoryService _categoryService,
        IWebHostEnvironment _env,
        ILogger<ProductController> _logger) : Controller
    {

        public async Task<IActionResult> Index(
            string? q,
            string sort = "newest",
            int? categoryId = null,
            int page = 1)
        {
            var vm = await _productService.GetAdminProductsAsync(q, sort, page, categoryId);
            return View(vm);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var product = await _productService.GetProductAsync(id.Value);
            if (product is null) return NotFound();
            return View(product);
        }


        public async Task<IActionResult> Create()
        {
            var vm = new ProductCreateUpdateVM { IsActive = true };
            await LoadCategoriesAsync(vm);
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(vm);
                return View(vm);
            }

            if (vm.ImageFile is not null)
            {
                var (ok, pathOrError) = await SaveImageAsync(vm.ImageFile);
                if (!ok)
                {
                    ModelState.AddModelError(nameof(vm.ImageFile), pathOrError);
                    await LoadCategoriesAsync(vm);
                    return View(vm);
                }
                vm.ImageUrl = pathOrError;
            }

            var (success, error) = await _productService.AddProductAsync(vm);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                await LoadCategoriesAsync(vm);
                return View(vm);
            }

            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest();
            var product = await _productService.GetProductAsync(id);
            if (product is null) return NotFound();

            var vm = new ProductCreateUpdateVM
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            };

            await LoadCategoriesAsync(vm);
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, ProductCreateUpdateVM vm)
        {
            if (id <= 0 || id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(vm);
                return View(vm);
            }

            if (vm.ImageFile is not null)
            {
                var (ok, pathOrError) = await SaveImageAsync(vm.ImageFile);
                if (!ok)
                {
                    ModelState.AddModelError(nameof(vm.ImageFile), pathOrError);
                    await LoadCategoriesAsync(vm);
                    return View(vm);
                }
                DeleteImageFile(vm.ImageUrl);
                vm.ImageUrl = pathOrError;
            }

            var (success, error) = await _productService.UpdateProductAsync(vm);
            if (!success)
            {
                await LoadCategoriesAsync(vm);
                ModelState.AddModelError(string.Empty, error);
                return View(vm);
            }

            TempData["SuccessMessage"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var product = await _productService.GetProductAsync(id.Value);
            bool result = await _productService.DeleteProductAsync(id.Value);

            if (result && product?.ImageUrl is not null)
                DeleteImageFile(product.ImageUrl);

            TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                result ? "Product deleted." : "Product not found.";

            return RedirectToAction(nameof(Index));
        }


        private async Task<(bool Ok, string PathOrError)> SaveImageAsync(IFormFile file)
        {
            const long MaxBytes = 5 * 1024 * 1024;
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext))
                return (false, $"Allowed image types: {string.Join(", ", allowed)}");

            if (file.Length > MaxBytes)
                return (false, "Image must be smaller than 5 MB.");

            var folder = Path.Combine(_env.WebRootPath, "images", "products");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var localPath = Path.Combine(folder, fileName);

            await using var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write);
            await file.CopyToAsync(fs);

            return (true, $"/images/products/{fileName}");
        }

        private void DeleteImageFile(string? relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl)) return;
            try
            {
                var localPath = Path.GetFullPath(
                    Path.Combine(
                        _env.WebRootPath,
                        relativeUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar)));

                if (!localPath.StartsWith(_env.WebRootPath, StringComparison.OrdinalIgnoreCase))
                    return;

                if (System.IO.File.Exists(localPath))
                    System.IO.File.Delete(localPath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not delete product image: {Url}", relativeUrl);
            }
        }

        private async Task LoadCategoriesAsync(ProductCreateUpdateVM vm)
        {
            var categories = await _categoryService.GetCategoriesAsync();
            vm.Categories = categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();
        }
    }
}
