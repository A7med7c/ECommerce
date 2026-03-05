using ECommerce.BLL.Enums;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController(IOrderService _orderService) : Controller
    {
        // ── GET /Admin/Order ──────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        // ── GET /Admin/Order/Details/5 ────────────────────────────────────
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order is null) return NotFound();
            return View(order);
        }

        // ── GET /Admin/Order/UpdateStatus/5 ──────────────────────────────
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order is null) return NotFound();

            var vm = new UpdateOrderStatusVM
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                CurrentStatus = order.Status,
                NewStatus = order.StatusValue   // pre-select current numeric value
            };
            return View(vm);
        }

        // ── POST /Admin/Order/UpdateStatus ────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var (success, error) = await _orderService.UpdateOrderStatusAsync(vm.OrderId, vm.NewStatus);
            if (!success)
            {
                TempData["Error"] = error;
                return View(vm);
            }

            TempData["Success"] = $"Order #{vm.OrderNumber} status updated.";
            return RedirectToAction(nameof(Index));
        }
    }
}
