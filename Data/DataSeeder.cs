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
       // --- CRÉATION DU COMPTE TECHNICIEN ---
        var techEmail = "tech@smartdesk.com";
        if (await userManager.FindByEmailAsync(techEmail) == null)
        {
            var techUser = new IdentityUser
            {
                UserName = techEmail,
                Email = techEmail,
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(techUser, "Smartdesk@2026");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(techUser, "Technicien");
            }
        }

        // --- CRÉATION DU COMPTE EMPLOYÉ ---
        var employeEmail = "employe@smartdesk.com";
        if (await userManager.FindByEmailAsync(employeEmail) == null)
        {
            var employeUser = new IdentityUser
            {
                UserName = employeEmail,
                Email = employeEmail,
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(employeUser, "Smartdesk@2026");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(employeUser, "Employe");
            }
        }

        
 
    }
}