using ECommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.PL.Controllers
{

    [Authorize(Roles = "Customer")]
    public class CustomerDashboardController(
        IOrderService _orderService) : Controller
    {
        private string CurrentUserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(CurrentUserId);
            return View(orders);
        }
    }
}
