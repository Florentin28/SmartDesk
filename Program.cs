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
        options.Password.RequiredLength = 12; // longueur minimale du MDP
        options.Password.RequiredUniqueChars = 3; // nombre minimal de caractères différents dans le MDP
        // --- Verrouillage du Compte (Lockout) ---
        // Active le verrouillage pour les nouveaux utilisateurs
        options.Lockout.AllowedForNewUsers = true;
        // Nombre maximum d'échecs avant blocage (ici 5 tentatives ratées)
        options.Lockout.MaxFailedAccessAttempts = 5;
        // Durée du blocage du compte (ici 15 minutes)
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
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
    options.LoginPath = "/";
    options.AccessDeniedPath = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Impose le flag Secure
    options.Cookie.HttpOnly = true;                          // Bloque l'accès JavaScript
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


// --- Middleware des En-têtes de Sécurité HTTP (Security Headers) ---
app.Use(async (context, next) =>
{
    // Empêche le reniflage de type MIME
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

    // Interdit l'affichage dans des frames/iframes (anti-Clickjacking)
    context.Response.Headers.Append("X-Frame-Options", "DENY");

    // Politique de référent : ne transmet l'origine que sur des liaisons HTTPS sécurisées
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

    // Content Security Policy adaptée à Blazor Server (SignalR, styles et scripts nécessaires)
    // Note : 'unsafe-inline' et 'unsafe-eval' sont requis par le moteur de rendu Blazor WebAssembly / Server refresh
    context.Response.Headers.Append("Content-Security-Policy",
        "default-src 'self'; " + // Règle de repli : restreint par défaut toutes les ressources non spécifiées au domaine local
        "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " + // Scripts du serveur autorisés (mots-clés unsafe nécessaires au moteur Blazor/SignalR)
        "style-src 'self' 'unsafe-inline'; " + // Feuilles de style locales et balises style en ligne autorisées
        "img-src 'self' data:; " + // Images locales (/uploads) et données encodées en base64 (icônes SVG) autorisées
        "connect-src 'self' wss: ws:; " + // Connexions réseau (fetch) et flux WebSockets requis pour le circuit temps réel Blazor Server
        "font-src 'self'; " + // Polices typographiques strictement restreintes au serveur local
        "frame-ancestors 'none';"); // Interdit catégoriquement d'embarquer l'application dans une iframe (anti-Clickjacking)

    await next();
});

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

