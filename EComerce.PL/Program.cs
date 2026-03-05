using EComerce.DAL.Data.Contexts;
using ECommerce.BLL.Mappings;
using ECommerce.BLL.Services.Classes;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.Validators;
using ECommerce.DAL.Entities.IdentityModule;
using ECommerce.DAL.Repositories.Classes;
using ECommerce.DAL.Repositories.Interfaces;
using ECommerce.PL.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI Container

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // AddDbContext => allow Di For DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });

            // ── ASP.NET Core Identity ─────────────────────────────────
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password policy
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                // Lockout policy
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Override cookie paths to MVC Account controller
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
            });

            // ── Unit of Work (replaces individual repository registrations) ──
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ── Application Services ──────────────────────────────────────
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IFavoriteService, FavoriteService>();

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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();   // ← must be after UseRouting, before endpoints

            app.UseAuthentication(); // ← must be before UseAuthorization
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // ── Seed Roles + Admin user ──────────────────────────────────────
            using (var scope = app.Services.CreateScope())
            {
                await DbSeeder.SeedAsync(
                    scope.ServiceProvider,
                    scope.ServiceProvider.GetRequiredService<IConfiguration>());
            }

            app.Run();
        }
    }
}
