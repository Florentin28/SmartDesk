namespace SmartDesk.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Commentaire { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int TicketId { get; set; } //  Clé étrangère vers le Ticket
        public Ticket? Ticket { get; set; } // Propriété de navigation vers l'objet Ticket
        public bool IsSatisfied { get; set; }
    
    }
}