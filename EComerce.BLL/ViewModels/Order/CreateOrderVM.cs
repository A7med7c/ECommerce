using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.ViewModels.Order
{
    /// <summary>
    /// View model for the Checkout / Place Order form.
    /// Carries the shipping address entered by the user and a read-only
    /// preview of the cart items (populated by the service, not the user).
    /// </summary>
    public class CreateOrderVM
    {
        // ── Shipping Address (user-entered) ───────────────────────────────

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street is required.")]
        [StringLength(200)]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "ZIP / Postal code is required.")]
        [StringLength(20)]
        [Display(Name = "ZIP / Postal Code")]
        public string Zip { get; set; } = string.Empty;

        // ── Cart summary (read-only, set by service) ──────────────────────

        public IReadOnlyList<CartPreviewItemVM> CartItems { get; set; } = [];

        public decimal Total => CartItems.Sum(i => i.LineTotal);
    }

    /// <summary>Individual cart row shown in the checkout summary.</summary>
    public class CartPreviewItemVM
    {
        public string ProductName { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
    }
}
