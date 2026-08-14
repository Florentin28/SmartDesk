namespace SmartDesk.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;

        // À quelle question appartient cette réponse ?
        public int QuestionId { get; set; }
        public Question? Question { get; set; }

        // OÙ MÈNE CETTE RÉPONSE ? (L'un des deux sera null)
        // 1. Soit vers une autre question :
        public int? NextQuestionId { get; set; }
        public Question? NextQuestion { get; set; }

        // 2. Soit vers la solution finale :
        public int? ProcedureId { get; set; }
        public Procedure? Procedure { get; set; }
    }
}