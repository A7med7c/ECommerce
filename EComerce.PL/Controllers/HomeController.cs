using ECommerce.BLL.Services.Interfaces;
using ECommerce.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EComerce.PL.Controllers
{
    public class HomeController(
        ILogger<HomeController> _logger,
        IProductService _productService) : Controller
    {


        public async Task<IActionResult> Index(
            int? categoryId,
            string? q,
            string sort = "newest",
            int page = 1)
        {
            var vm = await _productService.GetCatalogAsync(categoryId, q, sort, page);
            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
