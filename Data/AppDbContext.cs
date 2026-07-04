// Imports the Entity Framework Core framework which provides database automation tools.
using Microsoft.EntityFrameworkCore;
// Imports our own data models (where our Ticket class is defined) so this file can see it.
using SmartDesk.Models;

namespace SmartDesk.Data
{
    // The main database controller class. Inheriting from ': DbContext' gives it all EF Core database powers.
    public class AppDbContext : DbContext
    {
        // The constructor. It receives the configuration settings (like which database provider to use, e.g., SQLite) 
        // from Program.cs and forwards them to the base EF Core system using ': base(options)'.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Represents the 'Tickets' table in the database. Every row in this table will map to a C# 'Ticket' object.
        // '= null!;' tells the compiler that EF Core will initialize this property automatically, avoiding null warnings.
        public DbSet<Ticket> Tickets { get; set; } = null!;
    }
}