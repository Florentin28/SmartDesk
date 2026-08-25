namespace SmartDesk.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public bool IsSatisfied { get; set; }

        public string Commentaire { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Clé étrangère vers le ticket
        public int TicketId { get; set; }

        // Propriété de navigation
        public Ticket? Ticket { get; set; }
    }
}