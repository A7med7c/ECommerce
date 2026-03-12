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
        ILogger<CategoryController> _logger) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return View(categories);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var category = await _categoryService.GetCategoryAsync(id.Value);
            if (category is null) return NotFound();
            return View(category);
        }


        public async Task<IActionResult> Create()
        {
            var vm = await _categoryService.PrepareCreateVMAsync();
            return View(vm);
        }


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


        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest();
            var vm = await _categoryService.GetCategoryForEditAsync(id);
            if (vm is null) return NotFound();
            return View(vm);
        }


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


        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var category = await _categoryService.GetCategoryAsync(id.Value);
            if (category is null) return NotFound();
            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (id <= 0) return BadRequest();
                bool result = await _categoryService.DeleteCategoryAsync(id);

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
