using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.PL.Controllers
{


    [Authorize]
    public class OrderController(
        IOrderService _orderService,
        ICartService _cartService) : Controller
    {


        private string CurrentUserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;


        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(CurrentUserId);
            return View(orders);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var order = await _orderService.GetOrderByIdAsync(id.Value, CurrentUserId);
            if (order is null) return NotFound();

            return View(order);
        }


        public async Task<IActionResult> Create()
        {
            var cart = await _cartService.GetCartAsync();

            if (cart.IsEmpty)
            {
                TempData["ErrorMessage"] = "Your cart is empty. Add products before checking out.";
                return RedirectToAction("Index", "Cart");
            }

            var vm = _orderService.PrepareCheckoutVM(cart.Items);
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderVM vm)
        {
            var cart = await _cartService.GetCartAsync();


            vm.CartItems = cart.Items
                .Select(i => new CartPreviewItemVM
                {
                    ProductName = i.ProductName,
                    SKU = i.SKU,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })
                .ToList();

            if (cart.IsEmpty)
            {
                ModelState.AddModelError(string.Empty, "Your cart is empty.");
                return View(vm);
            }

            if (!ModelState.IsValid)
                return View(vm);

            var (success, error, orderId) =
                await _orderService.PlaceOrderAsync(vm, cart.Items, CurrentUserId);

            if (success)
            {
                await _cartService.ClearCartAsync();
                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction(nameof(Details), new { id = orderId });
            }


            ModelState.AddModelError(string.Empty, error);
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            bool cancelled = await _orderService.CancelOrderAsync(id, CurrentUserId);

            TempData[cancelled ? "SuccessMessage" : "ErrorMessage"] =
                cancelled ? "Order has been cancelled." : "Order not found.";

            return RedirectToAction(nameof(Index));
        }
    }
}
