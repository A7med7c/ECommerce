using EComerce.DAL.Data.Contexts;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.DAL.Repositories.Classes;
using ECommerce.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI Container

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // lifeTime[Objects] =>AddScoped , AddSingleton ,AddTransient

            //builder.Services.AddScoped<ApplicationDbContext>(); //// dosent support di

            // AddDbContext => allow Di For DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                //options.UseSqlServer("COnnectionString");
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts(); //Middleware that make ensure all requests int the deployment is https 
            }

            app.UseHttpsRedirection();
            // best practice to be before routing 
            app.UseStaticFiles();

            app.UseRouting();


            //app.UseAuthentication(); // must be before authorization.
            //app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
