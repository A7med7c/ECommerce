using ECommerce.DAL.Entities;
using ECommerce.DAL.Entities.IdentityModule;
using ECommerce.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.PL.Controllers
{
    [Authorize]
    public class ProfileController(
        UserManager<ApplicationUser> _userManager,
        SignInManager<ApplicationUser> _signInManager,
        ILogger<ProfileController> _logger) : Controller
    {


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await GetProfileVMAsync();
            if (vm is null) return NotFound();
            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var vm = await GetProfileVMAsync();
            if (vm is null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user is null)
                return NotFound();

            user.FullName = vm.FullName;
            user.PhoneNumber = vm.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(vm);
            }


            if (!string.IsNullOrWhiteSpace(vm.Country) || !string.IsNullOrWhiteSpace(vm.City)
                || !string.IsNullOrWhiteSpace(vm.Street) || !string.IsNullOrWhiteSpace(vm.Zip))
            {
                var defaultAddress = user.Addresses.FirstOrDefault(a => a.IsDefault);

                if (defaultAddress is null)
                {
                    defaultAddress = new Address
                    {
                        UserId = user.Id,
                        IsDefault = true,
                        CreatedAt  = DateTime.UtcNow
                    };
                    user.Addresses.Add(defaultAddress);
                }

                defaultAddress.Country = vm.Country ?? string.Empty;
                defaultAddress.City = vm.City ?? string.Empty;
                defaultAddress.Street = vm.Street ?? string.Empty;
                defaultAddress.Zip = vm.Zip ?? string.Empty;
                defaultAddress.ModifiedOn = DateTime.UtcNow;

                await _userManager.UpdateAsync(user);
            }

            _logger.LogInformation("User {Email} updated their profile.", user.Email);
            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(vm);
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User {Email} changed their password.", user.Email);
            TempData["Success"] = "Password changed successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<ProfileVM?> GetProfileVMAsync()
        {
            var user = await _userManager.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user is null) return null;

            var defaultAddress = user.Addresses.FirstOrDefault(a => a.IsDefault)
                              ?? user.Addresses.FirstOrDefault();

            return new ProfileVM
            {
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Country = defaultAddress?.Country,
                City = defaultAddress?.City,
                Street = defaultAddress?.Street,
                Zip = defaultAddress?.Zip
            };
        }
    }
}
