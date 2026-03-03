using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.PL.Controllers
{
    /// <summary>
    /// Handles order listing, details, checkout, and cancellation.
    /// Delegates all business logic to IOrderService — no domain logic lives here.
    /// </summary>
    public class OrderController(
        IOrderService _orderService,
        ICartService _cartService) : Controller
    {
        // ── GET /Order ────────────────────────────────────────────────────
        public IActionResult Index()
        {
            var orders = _orderService.GetAllOrders();
            return View(orders);
        }

        // ── GET /Order/Details/5 ──────────────────────────────────────────
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var order = _orderService.GetOrderById(id.Value);
            if (order is null) return NotFound();

            return View(order);
        }

        // ── GET /Order/Create ─────────────────────────────────────────────
        /// <summary>Show checkout form pre-populated with current cart.</summary>
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

        // ── POST /Order/Create ────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderVM vm)
        {
            var cart = await _cartService.GetCartAsync();

            // Re-populate cart preview (not posted back by form)
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

            int orderId = _orderService.PlaceOrder(vm, cart.Items);

            if (orderId > 0)
            {
                await _cartService.ClearCartAsync();
                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction(nameof(Details), new { id = orderId });
            }

            TempData["ErrorMessage"] = "Failed to place the order. Please try again.";
            return View(vm);
        }

        // ── POST /Order/Cancel ────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            bool cancelled = _orderService.CancelOrder(id);

            TempData[cancelled ? "SuccessMessage" : "ErrorMessage"] =
                cancelled ? "Order has been cancelled." : "Order not found.";

            return RedirectToAction(nameof(Index));
        }
    }
}
