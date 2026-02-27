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
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError(string.Empty, "Category can't be added");
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
        public ActionResult Edit(UpdateCategoryVM updateCategoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = _categoryService.UpdateCategory(updateCategoryVM);
                    if (result > 0)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError(string.Empty, "Category can't be updated");
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                        _logger.LogError($"Category can't be updated because{ex.Message}", ex);
                    else
                        _logger.LogError($"Category can't be updated because{ex}");
                }
            }
            ViewBag.ParentCategories = _categoryService.GetCategories();
            return View(updateCategoryVM);
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
