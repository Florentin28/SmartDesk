using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class IdentitySeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Define roles to seed
        var roles = new[] { "Admin", "Technicien", "Employe" };

        // Seed roles
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Define the admin user details
        var adminEmail = "admin@gmail.com";
        var adminPassword = "Admin@123";

        // Check if the admin user already exists
        var userExist = await userManager.FindByEmailAsync(adminEmail);
        if (userExist == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = adminEmail,
                PhoneNumber = "0412345678",
                EmailConfirmed = true
            };

            // Create the admin user
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                // Assign the Admin role to the user
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                throw new Exception("Failed to create the admin user: " + string.Join(", ", result.Errors));
            }
        }
    }
}