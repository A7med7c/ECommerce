using EComerce.DAL.Entities;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController(
        ICategoryService _categoryService,
        ILogger<Category> _logger) : Controller
    {
        // ── GET /Admin/Category ───────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return View(categories);
        }

        // ── GET /Admin/Category/Details/5 ─────────────────────────────────
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var category = await _categoryService.GetCategoryAsync(id.Value);
            if (category is null) return NotFound();
            return View(category);
        }

        // ── GET /Admin/Category/Create ────────────────────────────────────
        public async Task<IActionResult> Create()
        {
            var vm = await _categoryService.PrepareCreateVMAsync();
            return View(vm);
        }

        // ── POST /Admin/Category/Create ────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCategoryVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await _categoryService.PrepareCreateVMAsync();
                return View(vm);
            }

            try
            {
                int result = await _categoryService.AddCategoryAsync(vm);

                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Category created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Create failed.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                TempData["ErrorMessage"] = "Something went wrong.";
            }

            vm = await _categoryService.PrepareCreateVMAsync();
            return View(vm);
        }

        // ── GET /Admin/Category/Edit/5 ────────────────────────────────────
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest();
            var vm = await _categoryService.GetCategoryForEditAsync(id);
            if (vm is null) return NotFound();
            return View(vm);
        }

        // ── POST /Admin/Category/Edit/5 ───────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCategoryVM vm)
        {
            if (id <= 0 || id != vm.CategoryId) return BadRequest();

            if (!ModelState.IsValid)
            {
                vm = (await _categoryService.GetCategoryForEditAsync(id))!;
                return View(vm);
            }

            try
            {
                int result = await _categoryService.UpdateCategoryAsync(vm);
                TempData[result > 0 ? "SuccessMessage" : "ErrorMessage"] =
                    result > 0 ? "Category updated successfully!" : "Category not found.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category {Id}", id);
                TempData["ErrorMessage"] = "Something went wrong.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ── POST /Admin/Category/Delete ────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (!id.HasValue) return BadRequest();
                bool result = await _categoryService.DeleteCategoryAsync(id.Value);

                TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                    result ? "Category deleted successfully." : "Category not found.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category {Id}", id);
                TempData["ErrorMessage"] = "Something went wrong.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
