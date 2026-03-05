using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.PL.Controllers
{
    /// <summary>
    /// Customer-facing order controller.
    /// All actions require an authenticated user.
    /// Delegates all business logic to IOrderService — no domain logic lives here.
    /// </summary>
    [Authorize]
    public class OrderController(
        IOrderService _orderService,
        ICartService _cartService) : Controller
    {
        // ── Helpers ───────────────────────────────────────────────────────

        /// <summary>Returns the current user's identity string (never null when [Authorize] is active).</summary>
        private string CurrentUserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // ── GET /Order ────────────────────────────────────────────────────
        /// <summary>Lists only the orders that belong to the logged-in user.</summary>
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(CurrentUserId);
            return View(orders);
        }

        // ── GET /Order/Details/5 ──────────────────────────────────────────
        /// <summary>
        /// Shows full order detail.
        /// Returns 404 when the order doesn't exist OR belongs to a different user
        /// (prevents ID-tampering / IDOR).
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var order = await _orderService.GetOrderByIdAsync(id.Value, CurrentUserId);
            if (order is null) return NotFound();

            return View(order);
        }

        // ── GET /Order/Create (Checkout) ──────────────────────────────────
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

        // ── POST /Order/Create (Checkout submit) ──────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderVM vm)
        {
            var cart = await _cartService.GetCartAsync();

            // Re-populate cart preview (not posted back by the form)
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

            // Stock / availability error returned from service
            ModelState.AddModelError(string.Empty, error);
            return View(vm);
        }

        // ── POST /Order/Cancel ────────────────────────────────────────────
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
