using System.ComponentModel.DataAnnotations;

namespace ECommerce.PL.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        /// <summary>Safe return URL after login. Validated server-side via Url.IsLocalUrl().</summary>
        public string? ReturnUrl { get; set; }
    }
}
