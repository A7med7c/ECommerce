namespace EComerce.DAL.Entities
{
    public class BaseEntity // include the common properties 
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; } // user id 
        public DateTime? CreatedOn { get; set; } // the data of creating the record in the db 
        public int ModifiedBy { get; set; } // user id 
        public DateTime? ModifiedOn { get; set; } // the data of modifing the record in the db 
        public bool IsDeleted { get; set; } // used for soft deleting
    }
}
