using System;

namespace SmartDesk.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "New";
        
        // AJOUT : Nouvelle propriété pour la gestion des priorités
        public string Priority { get; set; } = "Medium"; 
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}