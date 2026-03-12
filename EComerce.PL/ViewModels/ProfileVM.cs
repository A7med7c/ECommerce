using System.ComponentModel.DataAnnotations;

namespace ECommerce.PL.ViewModels
{
    public class ProfileVM
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }


        [Display(Name = "Country")]
        public string? Country { get; set; }

        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "Street")]
        public string? Street { get; set; }

        [Display(Name = "Zip Code")]
        public string? Zip { get; set; }
    }
}
