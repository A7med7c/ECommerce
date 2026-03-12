using ECommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.PL.Controllers
{
    public class CartController(ICartService _cartService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            if (User.Identity is null || !User.Identity.IsAuthenticated)
            {


                var referer   = Request.Headers.Referer.ToString();
                var returnUrl = !string.IsNullOrEmpty(referer) && Url.IsLocalUrl(referer)
                    ? referer
                    : Url.Action("Index", "Catalog");

                var loginUrl = Url.Action("Login", "Account", new { returnUrl })!;


                if (IsAjaxRequest())
                    return Json(new { success = false, redirectUrl = loginUrl });


                return Redirect(loginUrl);
            }

            var (success, message) = await _cartService.AddToCartAsync(productId, quantity);

            if (IsAjaxRequest())
            {
                int cartCount = await _cartService.GetItemCountAsync();
                return Json(new { success, message, cartCount });
            }

            if (success)
                TempData["CartSuccess"] = message;
            else
                TempData["CartError"] = message;

            string? refererUrl = Request.Headers.Referer.ToString();
            if (!string.IsNullOrEmpty(refererUrl) && Url.IsLocalUrl(refererUrl))
                return Redirect(refererUrl);

            return RedirectToAction(nameof(Index));
        }


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            bool removed = await _cartService.RemoveFromCartAsync(productId);

            TempData[removed ? "CartSuccess" : "CartError"] =
                removed ? "Item removed from cart." : "Item not found in cart.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync();
            TempData["CartSuccess"] = "Cart cleared.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Count()
        {
            int count = await _cartService.GetItemCountAsync();
            return Json(new { count });
        }

        private bool IsAjaxRequest() =>
            Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
