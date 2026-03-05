using ECommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.PL.Controllers
{
    /// <summary>
    /// Handles all shopping-cart interactions.
    /// Delegates every business decision to ICartService — no logic lives here.
    /// </summary>
    public class CartController(ICartService _cartService) : Controller
    {
        // ── GET /Cart ────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            return View(cart);
        }

        // ── POST /Cart/Add ───────────────────────────────────────────────────
        /// <summary>Add a product to the cart. Requires authentication.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var (success, message) = await _cartService.AddToCartAsync(productId, quantity);

            if (success)
                TempData["CartSuccess"] = message;
            else
                TempData["CartError"] = message;

            // Return to the page the user came from if possible
            string? returnUrl = Request.Headers.Referer.ToString();
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        // ── POST /Cart/Update ────────────────────────────────────────────────
        /// <summary>Change the quantity of a cart line (qty = 0 removes it).</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int productId, int quantity)
        {
            var (success, message) = await _cartService.UpdateQuantityAsync(productId, quantity);

            if (success)
                TempData["CartSuccess"] = message;
            else
                TempData["CartError"] = message;

            return RedirectToAction(nameof(Index));
        }

        // ── POST /Cart/Remove ────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            bool removed = await _cartService.RemoveFromCartAsync(productId);

            TempData[removed ? "CartSuccess" : "CartError"] =
                removed ? "Item removed from cart." : "Item not found in cart.";

            return RedirectToAction(nameof(Index));
        }

        // ── POST /Cart/Clear ─────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync();
            TempData["CartSuccess"] = "Cart cleared.";
            return RedirectToAction(nameof(Index));
        }

        // ── GET /Cart/Count (AJAX helper) ────────────────────────────────────
        /// <summary>
        /// Lightweight endpoint used by the navbar badge to refresh the cart count
        /// without a full page reload. Returns: { "count": n }
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Count()
        {
            int count = await _cartService.GetItemCountAsync();
            return Json(new { count });
        }
    }
}
