using Microsoft.EntityFrameworkCore; // importe le framework de microsoft pour la gestion de BDD
using SmartDesk.Components; // importe les Pages et les layouts 
using Microsoft.AspNetCore.Identity; // Pour IdentityUser et IdentityRole
using SmartDesk.Data;                // Pour AppDbContext


var builder = WebApplication.CreateBuilder(args);

// Enregistre le service d'authentification pour gérer la session et la connexion des utilisateurs
builder.Services.AddSingleton<SmartDesk.Services.AuthService>();



builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurer et enregistrer le DbContext pour utiliser une base de données SQLite (smartdesk.db)
builder.Services.AddDbContext<SmartDesk.Data.AppDbContext>(options =>
    options.UseSqlite("Data Source=smartdesk.db"));

builder.Services.AddIdentityCore<IdentityUser>(options => 
{
        options.Password.RequireDigit = true; // au moins un chiffre dans le MDP
        options.Password.RequireLowercase = true; // au moins une lettre minuscule dans le MDP
        options.Password.RequireUppercase = true; // au moins une lettre majuscule dans le MDP
        options.Password.RequireNonAlphanumeric = true; // au moins un caractère spécial dans le MDP
        options.Password.RequiredLength = 6; // longueur minimale du MDP
})
.AddRoles<IdentityRole>() // différencier technicien de employé
.AddEntityFrameworkStores<AppDbContext>(); // relier Identity au fichier SQlite



var app = builder.Build();

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();

    // Apply pending migrations
    await dbContext.Database.MigrateAsync();

    // Seed roles and admin user
    await IdentitySeeder.SeedRolesAndAdminAsync(services);

    IdentitySeeder.SeedAkinator(dbContext);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Redirige vers la page d'erreur si la page demandée n'existe pas
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

