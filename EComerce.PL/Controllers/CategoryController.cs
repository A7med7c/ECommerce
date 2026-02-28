using EComerce.DAL.Entities;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.PL.Controllers
{
    public class CategoryController(ICategoryService _categoryService
        , IWebHostEnvironment _environment, ILogger<Category> _logger) : Controller
    {

        // GET: HomeController1
        public ActionResult Index()
        {
            var categories = _categoryService.GetCategories();
            return View(categories);
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            var category = _categoryService.GetCategory(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            ViewBag.categories = _categoryService.GetCategories();
            return View();
        }

        [HttpPost]
        public ActionResult Create(AddCategoryVM addCategoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = _categoryService.AddCategory(addCategoryVM);

                    if (result > 0)
                    {
                        TempData["SuccessMessage"] = "Category created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Category can't be added");
                    }
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                        _logger.LogError($"Category can't be created because{ex.Message}", ex);
                    else
                        _logger.LogError($"Category can't be created because{ex}");
                }
            }

            ViewBag.ParentCategories = _categoryService.GetCategories();
            return View(addCategoryVM);
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            var cateogry = _categoryService.GetCategoryForEdit(id);

            if (cateogry != null)
            {
                ViewBag.ParentCategories = _categoryService.GetCategories();
                return View(cateogry);
            }
            return NotFound();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        public IActionResult Edit(UpdateCategoryVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                int result = _categoryService.UpdateCategory(vm);

                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Category updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Update failed.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category");
                TempData["ErrorMessage"] = "Something went wrong.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                bool result = _categoryService.DeleteCategory(id);

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
