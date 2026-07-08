using Microsoft.EntityFrameworkCore;
using SmartDesk.Models;

namespace SmartDesk.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // AJOUT : Force Entity Framework à créer le fichier ET les tables s'ils n'existent pas
            Database.EnsureCreated();
        }

        public DbSet<Ticket> Tickets { get; set; }
    }
}