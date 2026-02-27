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
            return View();
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

                    ModelState.AddModelError(string.Empty, "Category can't be added");
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                        _logger.LogError(ex, ex.Message);
                    else
                        _logger.LogError("Category can't be added");
                }
            }

            ViewBag.categories = _categoryService.GetCategories();
            return View(addCategoryVM);
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
