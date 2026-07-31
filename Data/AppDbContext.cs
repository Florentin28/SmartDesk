using Microsoft.EntityFrameworkCore; // importe le framework de microsoft pour la gestion de BDD
using SmartDesk.Models; //importe tickets et procedures pour les utiliser dans le DbContext
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; //importe le framework de microsoft pour la gestion de l'identité et des utilisateurs

namespace SmartDesk.Data
{
    // AppDbContext hérite de IdentityDbContext pour gérer l'authentification et l'autorisation des utilisateurs.
    public class AppDbContext : IdentityDbContext
    {
        // Le constructeur de AppDbContext prend des options de configuration pour le DbContext.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
          
        }

        // DbSet<Ticket> Chaque ligne de la table Tickets dans la BDD sera une instance de la classe Ticket.
        public DbSet<Ticket> Tickets { get; set; }

        // DbSet<Procedure> Chaque ligne de la table Procedures dans la BDD sera une instance de la classe Procedure.
        public DbSet<Procedure> Procedures { get; set; }
    }
}