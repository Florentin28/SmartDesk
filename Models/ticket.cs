using System;

namespace SmartDesk.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "New";
        public string Priority { get; set; } = "Medium";
        
        // AJOUT : Stocke l'email de l'employé qui a créé le ticket
        public string SubmittedBy { get; set; } = string.Empty; 
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}