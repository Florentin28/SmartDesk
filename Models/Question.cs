namespace SmartDesk.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        
        // Relation : Une question a plusieurs réponses
        public List<Answer> Answers { get; set; } = new();
    }
}