using ECommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.PL.Controllers
{


    [Authorize]
    public class FavoriteController(IFavoriteService _favoriteService) : Controller
    {
        private string CurrentUserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;


        public async Task<IActionResult> Index()
        {
            var favorites = await _favoriteService.GetFavoritesAsync(CurrentUserId);
            return View(favorites);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId)
        {
            var (success, message) = await _favoriteService.AddAsync(CurrentUserId, productId);

            if (IsAjaxRequest())
                return Json(new { success, message, isFavorite = true });

            TempData[success ? "FavSuccess" : "FavError"] = message;


            string? returnUrl = Request.Headers.Referer.ToString();
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            bool removed = await _favoriteService.RemoveAsync(CurrentUserId, productId);

            if (IsAjaxRequest())
                return Json(new
                {
                    success = removed,
                    message = removed ? "Removed from your favourites." : "Item not found in favourites.",
                    isFavorite = false
                });

            TempData[removed ? "FavSuccess" : "FavError"] =
                removed ? "Removed from your favourites." : "Item not found in favourites.";

            string? returnUrl = Request.Headers.Referer.ToString();
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        private bool IsAjaxRequest() =>
            Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
