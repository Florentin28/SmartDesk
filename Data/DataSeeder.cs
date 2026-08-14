using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
namespace SmartDesk.Data;
using SmartDesk.Models;



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

    public static void SeedAkinator(AppDbContext context)
{
    // Si on a déjà des questions, on ne fait rien
    if (context.Questions.Any()) return;

    // 1. On crée les solutions finales (Procédures)
    var procScreen = new Procedure { Title = "Vérifier les câbles", Content = "Débranchez et rebranchez le câble VGA/HDMI et l'alimentation de l'écran." };
    var procMouse = new Procedure { Title = "Changer les piles", Content = "Remplacez les piles de la souris sans fil et vérifiez le dongle USB." };
    var procNetwork = new Procedure { Title = "Redémarrer le routeur", Content = "Débranchez le routeur, attendez 30 secondes, puis rebranchez-le." };

    context.Procedures.AddRange(procScreen, procMouse, procNetwork);
    context.SaveChanges();

    // 2. On crée les questions de notre arbre
    var q1 = new Question { Text = "Quel est votre problème principal ?" };
    var q2 = new Question { Text = "Votre écran s'allume-t-il (petite lumière visible) ?" };
    var q3 = new Question { Text = "S'agit-il d'un problème matériel ou logiciel ?" };

    context.Questions.AddRange(q1, q2, q3);
    context.SaveChanges();

    // 3. On crée les réponses (les ponts qui relient tout)
    var answers = new List<Answer>
    {
        // Réponses à la Question 1
        new Answer { Text = "Problème d'écran", QuestionId = q1.Id, NextQuestionId = q2.Id },
        new Answer { Text = "Problème de souris", QuestionId = q1.Id, ProcedureId = procMouse.Id },
        new Answer { Text = "Problème de réseau", QuestionId = q1.Id, ProcedureId = procNetwork.Id },
        new Answer { Text = "Autre problème matériel", QuestionId = q1.Id, NextQuestionId = q3.Id },

        // Réponses à la Question 2 (Suite de l'écran)
        new Answer { Text = "Oui, mais il affiche 'No Signal'", QuestionId = q2.Id, ProcedureId = procScreen.Id },
        new Answer { Text = "Non, aucune lumière", QuestionId = q2.Id, ProcedureId = procScreen.Id },

        // Réponses à la Question 3 (Autre)
        new Answer { Text = "C'est matériel, aidez-moi !", QuestionId = q3.Id, ProcedureId = procScreen.Id }
    };

    context.Answers.AddRange(answers);
    context.SaveChanges();
}
}