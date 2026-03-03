using EComerce.DAL.Entities;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.PL.Controllers
{
    public class CategoryController(ICategoryService _categoryService
        , IWebHostEnvironment _environment, ILogger<Category> _logger) : Controller
    {

        // GET
        public ActionResult Index()
        {
            var categories = _categoryService.GetCategories();
            return View(categories);
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var category = _categoryService.GetCategory(id.Value);

            if (category == null)
                return NotFound();
            return View(category);
        }

        // GET
        public IActionResult Create()
        {
            var vm = _categoryService.PrepareCreateVM();
            return View(vm);
        }


        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddCategoryVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm = _categoryService.PrepareCreateVM();
                return View(vm);
            }

            try
            {
                int result = _categoryService.AddCategory(vm);

                if (result > 0)
                {
                    TempData["SuccessMessage"] =
                        "Category created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Create failed.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                TempData["ErrorMessage"] =
                    "Something went wrong.";
            }

            vm = _categoryService.PrepareCreateVM();
            return View(vm);
        }
        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            var vm = _categoryService.GetCategoryForEdit(id);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UpdateCategoryVM vm)
        {
            if (id <= 0)
                return BadRequest();

            if (id != vm.CategoryId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                vm = _categoryService.GetCategoryForEdit(id)!;
                return View(vm);
            }

            try
            {
                int result = _categoryService.UpdateCategory(vm);

                if (result == 0)
                {
                    TempData["ErrorMessage"] =
                        "Category not found.";
                }
                else
                {
                    TempData["SuccessMessage"] =
                        "Category updated successfully!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating category with id {CategoryId}", id);

                TempData["ErrorMessage"] =
                    "Something went wrong.";
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
                bool result = _categoryService.DeleteCategory(id.Value);

                if (!result)
                {
                    _logger.LogWarning($"Delete failed: Category Id = {id}");
                    TempData["ErrorMessage"] = "Category not found.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Category deleted successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                    _logger.LogError(ex, $"Error deleting category {id}");
                else
                    _logger.LogError("Error deleting category");

                TempData["ErrorMessage"] = "Something went wrong.";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
