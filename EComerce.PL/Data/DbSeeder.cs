using ECommerce.DAL.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.PL.Data
{
    /// <summary>
    /// Seeds the Admin role and a default Admin user on first startup.
    /// Credentials are read from IConfiguration ("AdminSeed" section)
    /// so they can be overridden per environment without touching code.
    /// </summary>
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration config)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            await SeedRolesAsync(roleManager, logger);
            await SeedAdminUserAsync(userManager, config, logger);
        }

        // ── Roles ─────────────────────────────────────────────────────────

        private static readonly string[] Roles = { "Admin", "Customer" };

        private static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager,
            ILogger logger)
        {
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (result.Succeeded)
                        logger.LogInformation("Seeded role: {Role}", role);
                    else
                        logger.LogError("Failed to seed role {Role}: {Errors}",
                            role, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        // ── Admin User ────────────────────────────────────────────────────

        private static async Task SeedAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            IConfiguration config,
            ILogger logger)
        {
            var section = config.GetSection("AdminSeed");
            var email = section["Email"] ?? "admin@ecommerce.com";
            var password = section["Password"] ?? "Admin@123!";
            var fullName = section["FullName"] ?? "System Administrator";

            if (await userManager.FindByEmailAsync(email) is not null)
                return; // already seeded

            var admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true        // skip e-mail confirmation for seed account
            };

            var result = await userManager.CreateAsync(admin, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
                logger.LogInformation("Seeded Admin user: {Email}", email);
            }
            else
            {
                logger.LogError("Failed to seed Admin user: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
