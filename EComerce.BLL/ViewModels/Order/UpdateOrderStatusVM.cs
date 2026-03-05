using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.ViewModels.Order
{
    /// <summary>
    /// Carries the new status value for Admin/Order/UpdateStatus POST.
    /// </summary>
    public class UpdateOrderStatusVM
    {
        [Required]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Please select a status.")]
        public int NewStatus { get; set; }

        // Read-only fields re-populated for the view
        public string OrderNumber { get; set; } = null!;
        public string CurrentStatus { get; set; } = null!;
    }
}
