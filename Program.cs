using Microsoft.EntityFrameworkCore; // importe le framework de microsoft pour la gestion de BDD
using SmartDesk.Components; // importe les Pages et les layouts 

var builder = WebApplication.CreateBuilder(args);

// Enregistre le service d'authentification pour gérer la session et la connexion des utilisateurs
builder.Services.AddScoped<SmartDesk.Services.AuthService>();



builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurer et enregistrer le DbContext pour utiliser une base de données SQLite (smartdesk.db)
builder.Services.AddDbContext<SmartDesk.Data.AppDbContext>(options =>
    options.UseSqlite("Data Source=smartdesk.db"));


var app = builder.Build();

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

