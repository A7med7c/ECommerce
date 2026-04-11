namespace EComerce.DAL.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

    }
}
