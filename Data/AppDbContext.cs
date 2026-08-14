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

        public DbSet<Procedure> Procedures { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
        base.OnModelCreating(modelBuilder);

        // Dit à la base de données de ne pas tout effacer en chaîne si on supprime une question
         modelBuilder.Entity<Answer>()
        .HasOne(a => a.NextQuestion)
        .WithMany()
        .HasForeignKey(a => a.NextQuestionId)
        .OnDelete(DeleteBehavior.Restrict);
}
    }
}