using Microsoft.EntityFrameworkCore; // importe le framework de microsoft pour la gestion de BDD
using SmartDesk.Components; // importe les Pages et les layouts 
using Microsoft.AspNetCore.Identity; // Pour IdentityUser et IdentityRole
using SmartDesk.Data;                // Pour AppDbContext
using SmartDesk.Services;
using Microsoft.AspNetCore.RateLimiting; // pour limiter le nombre de requêtes par utilisateur


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<SmartDesk.Services.AuthService>();



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
.AddSignInManager() // pour l'authentification
.AddEntityFrameworkStores<AppDbContext>(); // relier Identity au fichier SQlite

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
})
.AddIdentityCookies();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/"; // Redirige vers l'accueil pour se connecter
    options.AccessDeniedPath = "/"; // Évite le 404 en cas de droits insuffisants
});

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<EmailService>(); // pour l'envoi de mail

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, cancellationToken) =>
    {
        // On redirige vers l'accueil avec un paramètre d'erreur dédié
        context.HttpContext.Response.Redirect("/?error=rate-limit");
        await Task.CompletedTask;
    };
    options.AddFixedWindowLimiter("login-policy", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });
});


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
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapPost("/login", async (
    HttpContext context,
    [Microsoft.AspNetCore.Mvc.FromForm] string email,
    [Microsoft.AspNetCore.Mvc.FromForm] string password,
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync(email);
    if (user != null)
    {
        var result = await signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return Results.LocalRedirect("/");
        }
    }

    return Results.LocalRedirect("/?error=1");
})
.RequireRateLimiting("login-policy"); // 🛡️ Protection anti-brute-force

app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.LocalRedirect("/");
});

app.Run();

