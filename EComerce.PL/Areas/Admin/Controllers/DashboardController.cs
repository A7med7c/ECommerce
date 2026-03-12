using ECommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController(
        IProductService _productService,
        IOrderService _orderService,
        ICategoryService _categoryService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var productList = await _productService.GetAdminProductsAsync("", "newest", 1, null, 1);
            var orders = await _orderService.GetAllOrdersAsync();
            var categories = await _categoryService.GetCategoriesAsync();

            ViewBag.TotalProducts = productList.TotalCount;
            ViewBag.TotalOrders = orders.Count();
            ViewBag.TotalCategories = categories.Count();

            return View();
        }
    }
}
