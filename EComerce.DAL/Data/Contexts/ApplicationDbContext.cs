namespace EComerce.DAL.Data.Contexts
{
        //CLR Who Will Create And Pass Options
        // C# 12 .Net 8
        // PRimary Constructor adds constrains that when creating another constructor must chain on it 
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
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
        }
        public DbSet<Category> Categories { get; set; }
    }
}
