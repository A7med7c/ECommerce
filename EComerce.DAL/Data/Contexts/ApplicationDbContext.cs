using ECommerce.DAL.Entities;
using ECommerce.DAL.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EComerce.DAL.Data.Contexts
{
        //CLR Who Will Create And Pass Options
        // C# 12 .Net 8
        // PRimary Constructor adds constrains that when creating another constructor must chain on it 
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        #region Tightly Coupled Way

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionstring");
        //}

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
