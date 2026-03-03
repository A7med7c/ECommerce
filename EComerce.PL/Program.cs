using EComerce.DAL.Data.Contexts;
using ECommerce.BLL.Mappings;
using ECommerce.BLL.Services.Classes;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.Validators;
using ECommerce.DAL.Repositories.Classes;
using ECommerce.DAL.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
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

            // ── Unit of Work (replaces individual repository registrations) ──
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ── Application Services ──────────────────────────────────────
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // ── Shopping Cart ─────────────────────────────────────────────
            // IHttpContextAccessor: lets BLL services reach the HTTP session
            // without depending on Controller/ControllerBase directly.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICartService, CartService>();

            // ── Session (in-process; swap DistributedCache for Redis in prod) ──
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;   // JS cannot read the cookie
                options.Cookie.IsEssential = true;   // GDPR: required for function
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });

            // ── AutoMapper (scans BLL assembly for all Profile classes) ───
            builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);

            // ── FluentValidation (scans BLL assembly for all validators) ──
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<AddCategoryValidator>();
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

            app.UseSession();   // ← must be after UseRouting, before endpoints

            //app.UseAuthentication(); // must be before authorization.
            //app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
