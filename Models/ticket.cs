namespace SmartDesk.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // Default to empty string
        public string Description { get; set; } = string.Empty; // Default to empty string
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; } = "New"; // Default status
    }
}