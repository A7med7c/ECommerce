namespace ECommerce.PL.ViewModels
{
    public class UserWithRoleViewModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = "No Role";
        public bool IsActive { get; set; } = true;
        public bool HasRelatedData { get; set; }
    }
}
