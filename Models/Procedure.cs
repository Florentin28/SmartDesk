namespace SmartDesk.Models
{
    public class Procedure
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int SuccessCount { get; set; } = 0; // Le compteur de victoires
    }
}