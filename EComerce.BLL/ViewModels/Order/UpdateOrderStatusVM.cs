using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.ViewModels.Order
{


    public class UpdateOrderStatusVM
    {
        [Required]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Please select a status.")]
        public int NewStatus { get; set; }


        public string OrderNumber { get; set; } = null!;
        public string CurrentStatus { get; set; } = null!;
    }
}
