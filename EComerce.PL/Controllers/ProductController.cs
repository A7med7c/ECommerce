using ECommerce.BLL.Services.Interfaces;
using ECommerce.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.PL.Controllers
{
    public class ProductController(IProductService _productService, ICategoryService _categoryService
        , IWebHostEnvironment _environment, ILogger<Product> _logger) : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var products = _productService.GetProducts();
            return View(products);
        }
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var product = _productService.GetProduct(id.Value);

            if (product == null)
                return NotFound();
            return View(product);
        }

        // GET
        public IActionResult Create()
        {
            var vm = new ProductCreateUpdateVM();
            LoadCategories(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                LoadCategories(vm);
                return View(vm);
            }

            try
            {
                var result = _productService.AddProduct(vm);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Can't Create New Product");
                    LoadCategories(vm);
                    return View(vm);
                }

                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError("", "Something went wrong.");
                LoadCategories(vm);
                return View(vm);
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            var product = _productService.GetProduct(id);

            if (product == null)
                return NotFound();

            var vm = new ProductCreateUpdateVM
            {
                Id = id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId
            };

            LoadCategories(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, ProductCreateUpdateVM vm)
        {
            if (id <= 0)
                return BadRequest();

            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                LoadCategories(vm);
                return View(vm);
            }

            try
            {
                bool result = _productService.UpdateProduct(vm);

                if (!result)
                {
                    LoadCategories(vm);
                    ModelState.AddModelError(string.Empty, "Product not found.");
                    return View(vm);
                }

                TempData["SuccessMessage"] = "Product updated successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with id {ProductId}", id);
                LoadCategories(vm);
                ModelState.AddModelError(string.Empty, "Something went wrong.");
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (!id.HasValue) return BadRequest();
                bool result = _productService.DeleteProduct(id.Value);

                if (!result)
                {
                    _logger.LogWarning($"Delete failed: Product Id = {id}");
                    TempData["ErrorMessage"] = "Product not found.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Product deleted successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                    _logger.LogError(ex, $"Error deleting Product {id}");
                else
                    _logger.LogError("Error deleting Product");

                TempData["ErrorMessage"] = "Something went wrong.";

                return RedirectToAction(nameof(Index));
            }
        }

        private void LoadCategories(ProductCreateUpdateVM vm)
        {
            var categories = _categoryService.GetCategories();

            vm.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }
    }
}
