using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Entities.IdentityModule;
using ECommerce.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController(
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager,
        ApplicationDbContext _dbContext) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var viewModels = new List<UserWithRoleViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                bool hasOrders = await _dbContext.Orders.AnyAsync(o => o.UserId == user.Id);
                bool hasFavorites = await _dbContext.Favorites.AnyAsync(f => f.UserId == user.Id);

                viewModels.Add(new UserWithRoleViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    FullName = user.FullName,
                    Role = roles.FirstOrDefault() ?? "No Role",
                    IsActive = user.IsActive,
                    HasRelatedData = hasOrders || hasFavorites
                });
            }

            return View(viewModels);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound();

            if (!await _roleManager.RoleExistsAsync(role))
                return RedirectToAction(nameof(Index));

            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RevokeRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(user, role);

            return RedirectToAction(nameof(Details), new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            if (await IsLastAdminAsync(user))
                return RedirectToAction(nameof(Index));

            user.IsActive = false;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            user.IsActive = true;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            if (await IsLastAdminAsync(user))
            {
                TempData["Error"] = "Cannot delete the last admin user.";
                return RedirectToAction(nameof(Index));
            }

            bool hasOrders = await _dbContext.Orders.AnyAsync(o => o.UserId == user.Id);

            if (hasOrders)
            {
                TempData["Error"] = "Cannot delete this customer because they have existing orders.";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IsLastAdminAsync(ApplicationUser user)
        {
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
                return false;

            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return admins.Count == 1 && admins[0].Id == user.Id;
        }
    }
}
